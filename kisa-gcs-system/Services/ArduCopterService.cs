using System.Text.Json;
using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Models;
using kisa_gcs_system.Services.Helper;
using MAVSDK;

using static kisa_gcs_system.Models.ArrowButton;

namespace kisa_gcs_system.Services;

/*
 * STX: 0xFD로 고정된 패킷 시작 마커
 * LEN: "PAYLOAD" 부분의 길이
 * INC FLAGS: MAVLINK 호환 플래그, 이해할 수 없는 플래그를 가진 패킷은 버려진다. 보통은 0x00 이다.
 * CMP FLAGS: MAVLINK 비호환 플래그, 이해할 수 없는 플래그를 가진 패킷도 처리된다. 비표준 구현체등에 사용도리 수 있다. 보통 0x00이다.
 * SEQ: 메세지의 시퀀스 번호
 * SYS ID: 송신자의 시스템 ID, 용도나 구현체에 따라서 임의로 지정 / 시뮬레이터에서는 인스턴스를 다르게 설정해도 SYS ID가 모두 1이라서 다른 구분 방법 필요
 * COMP ID: 송신자의 컴포넌트 ID, 용도나 구현체에 따라서 임의로 지정
 * MSG ID: 3바이트로 구성된 메시지 ID, 메시지의 의미 나타냄
 * PAYLOAD: 메세지의 실제 데이터 (최대 255 바이트)
 * CHECKSUM: 메시지의 CRC 체크섬
 * SIGNATURE: 메세지의 서명 (보통은 생략)
 */

public class ArduCopterService : Hub<IDroneHub>
{
    private readonly IHubContext<ArduCopterService> _hubContext;
    private IChannelHandlerContext? _context;
    private IPEndPoint? _droneAddress;
    
    private readonly MAVLink.MavlinkParse _parser = new();
    private readonly MavlinkMapper _mapper = new();
    
    private readonly GcsApiService _gcsApiService;
        
    private static int ThrottleIncrement = 300;
    private static int yawIncrement = 50;
    // private static int SpeedIncrement = 50;
    
    private VincentyCalculator _vincentyCalculator = new();
    
    public ArduCopterService(IHubContext<ArduCopterService> hubContext, GcsApiService gcsApiService)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _gcsApiService = gcsApiService;
    }

    // 드론 상태 정보 내보내기 
    public async Task HandleMavlinkMessage(MAVLink.MAVLinkMessage msg, IChannelHandlerContext ctx, IPEndPoint droneAddress)
    { 
        _context = ctx; 
        _droneAddress = droneAddress;
        
        string droneId = msg.sysid.ToString();
        _mapper.setDroneId(droneId);
        
        if ((MAVLink.MAVLINK_MSG_ID)msg.msgid == MAVLink.MAVLINK_MSG_ID.STATUSTEXT)
        {
            var logdata = (MAVLink.mavlink_statustext_t)msg.data;
            var text = string.Join("", logdata.text.Select(c => (char)c));
            
            if (text.StartsWith("Disarming motors"))
            {
                _mapper.CompleteMission();
            }

            _mapper.UpdateDroneLogger(text);
        }
        
        object data = msg.data;
        _mapper.UpdateDroneState(data);
        
        string droneMessage = _mapper.ObjectToJson();
        await _hubContext.Clients.All.SendAsync("droneMessage", droneMessage);
    }

    public async Task HandleDroneContorlStt(string controlStt)
    {
        _mapper.setControlStt(controlStt);
    }

    // 비행 모드 변경 메소드 (Auto ~ RTL)
    public async Task HandleDroneFlightMode(CustomMode flightMode)
    {
        // MAVLink 프로토콜에서 사용되는 메시지 및 명령 생성
        var commandBody = new MAVLink.mavlink_set_mode_t()
        { 
            // 시뮬레이터는 SYS ID 가 1 이어서?
            target_system = (byte)1,
            custom_mode = (uint)flightMode,
            base_mode = 1,
        };
         
        // 생성된 명령을 이용하여 MAVLink 메시지 생성 
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.SET_MODE, commandBody));
         
        // 생성된 MAVLink 메시지를 이용하여 드론에 비행 모드 변경 명령을 비동기적으로 전송 
        await SetCommandAsync(msg);
     }

    // 비행 명령 메소드 (Arm ~ Land)
    public async Task HandleDroneFlightCommand(DroneFlightCommand flightCommand)
    {
        try
        {
            MAVLink.mavlink_command_long_t? commandBody = null;
            MAVLink.mavlink_set_mode_t? setModeMsg = null;

        switch (flightCommand)
        {
            case DroneFlightCommand.ARM:
            {
                commandBody = new MAVLink.mavlink_command_long_t()
                {
                    command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM,
                    param1 = 1 // arm
                };
                break;
            }
            
            case DroneFlightCommand.DISARM:
            {
                commandBody = new MAVLink.mavlink_command_long_t()
                {
                    command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM,
                    param1 = 0,     // dis-arm
                    param2 = 21196  // 펌웨어에 따라 다른 값일 수 있음 
                };
                break;
            }
            
            case DroneFlightCommand.TAKEOFF:
            {
                int alt = _mapper.getMissionAlt();
                if ((_mapper.getRelativeAlt() < 0.5) && (_mapper.getFlightMode() == CustomMode.GUIDED))
                {
                    _mapper.StartMission();
                    commandBody = new MAVLink.mavlink_command_long_t()
                    {
                        command = (ushort)MAVLink.MAV_CMD.TAKEOFF,
                        param1 = 0,         // pitch(rad), 드론의 전방 기울기 각도 
                        param3 = (float)5,  // ascend rate (m/s), 이륙 중에 드론이 수직으로 상승하는 속도
                        param4 = 0,         // yaw(rad), 드론의 회전을 나타내는 각도
                        param5 = 0,         // x, 드론의 이륙 위치 x
                        param6 = 0,         // y, 드론의 이륙 위치 y
                        param7 = alt,        // z(m), 드론의 이륙 높이(미터) 
                };
                }
                break;
            }
            case DroneFlightCommand.LAND:
            {
                setModeMsg = new MAVLink.mavlink_set_mode_t()
                {
                    base_mode = 1,
                    custom_mode = (byte)CustomMode.LAND,
                    target_system = 1
                };
                break;
            }
        }
        var msg = commandBody != null
            ? new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
                MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, 
                commandBody))
            : new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
                MAVLink.MAVLINK_MSG_ID.SET_MODE,
                setModeMsg));
        
        await SetCommandAsync(msg); 
        // Console.WriteLine($"Drone flight command '{flightCommand}' successfully executed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling drone flight command '{flightCommand}': {ex.Message}");
        }
    }

    // 미션 부여하기 
    public async Task HandleDroneStartMarking(double lat, double lng)
    {
        _mapper.setStartPoint(lat, lng);
    }

    public async Task HandleDroneTargetMarking(double lat, double lng)
    {
        _mapper.setTargetPoint(lat, lng);
    }

    public async Task HandleDroneTransitMarking(object transitList)
    {
        if (transitList is JsonElement jsonElement)
        {
            List<DroneLocation> transitPoints = new List<DroneLocation>();

            // JsonElement이면서 배열
            foreach (JsonElement arrayElement in jsonElement.EnumerateArray())
            {
                DroneLocation location = ParseDroneLocation(arrayElement);
                transitPoints.Add(location);
            }

            _mapper.setTransitPoint(transitPoints);
        }
    }

    private DroneLocation ParseDroneLocation(JsonElement jsonElement)
    {
        DroneLocation location = new DroneLocation();

        if (jsonElement.TryGetProperty("lat", out var latElement) && latElement.ValueKind == JsonValueKind.Number)
        {
            location.lat = latElement.GetDouble();
        }

        if (jsonElement.TryGetProperty("lng", out var lngElement) && lngElement.ValueKind == JsonValueKind.Number)
        {
            location.lng = lngElement.GetDouble();
        }

        if (jsonElement.TryGetProperty("global_frame_alt", out var globalFrameAltElement) && globalFrameAltElement.ValueKind == JsonValueKind.Number)
        {
            location.global_frame_alt = globalFrameAltElement.GetDouble();
        }

        if (jsonElement.TryGetProperty("terrain_alt", out var terrainAltElement) && terrainAltElement.ValueKind == JsonValueKind.Number)
        {
            location.terrain_alt = terrainAltElement.GetDouble();
        }

        return location;
    }

    public async Task HandleMissionAlt(short missionalt)
    {
        _mapper.setMissionAlt(missionalt);
    }

    public async Task HandleDroneMoveToMission(string startPoint, string targetPoint, List<string> transitPoint, int alt, string totalDistance)
    {
        try
        {
            List<double>? StartPoint = _gcsApiService.GetLocalPoint(startPoint);
            if (_vincentyCalculator.DistanceCalculater(_mapper.getCurrentLat(), _mapper.getCurrentLon(),
                    StartPoint[0], StartPoint[1]) > 1.5)
            {
                throw new Exception("출발 지점이 잘못 입력되었습니다.");
            }

            string trimmedTotalDistance = totalDistance.Substring(0, totalDistance.Length - 3);
            string numericPart = new string(trimmedTotalDistance.Where(char.IsDigit).ToArray());
            double TotalDistance = double.Parse(numericPart);

            _mapper.setControlStt("auto");
            _mapper.setCurrentDistance(0);
            _mapper.setMissionAlt(alt);
            _mapper.setTotalDistance(TotalDistance);

            HandleDroneFlightMode(CustomMode.GUIDED);
            
            HandleDroneFlightCommand(DroneFlightCommand.ARM);
            Thread.Sleep(1000);
            
            HandleDroneFlightCommand(DroneFlightCommand.TAKEOFF);
            Thread.Sleep(1000);
            while (_mapper.getRelativeAlt() < _mapper.getMissionAlt() - 0.1) { }
            Console.WriteLine("Mission Alt Reached");

            for (int i = 0; i < transitPoint.Count; i++)
            {
                List<double>? TransitPoint = _gcsApiService.GetLocalPoint(transitPoint[i]);
                await sendMission(TransitPoint[0], TransitPoint[1], alt);
                _mapper.setPathIndex(i+1);
                Thread.Sleep(1000);
                while (_vincentyCalculator.DistanceCalculater(_mapper.getCurrentLat(), _mapper.getCurrentLon(),
                           TransitPoint[0], TransitPoint[1]) > 1.5)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Q)
                        {
                            Console.WriteLine("BRAKE");
                            HandleDroneFlightMode(CustomMode.BRAKE);
                            return;
                        }
                    }
                }
                Console.WriteLine($"Transit{i} Arrival");
            }
            
            List<double>? TargetPoint = _gcsApiService.GetLocalPoint(targetPoint);
            await sendMission(TargetPoint[0], TargetPoint[1], alt);
            _mapper.setPathIndex(transitPoint.Count+1);
            Thread.Sleep(1000);
            while (_vincentyCalculator.DistanceCalculater(_mapper.getCurrentLat(), _mapper.getCurrentLon(),
                       TargetPoint[0], TargetPoint[1]) > 1.5)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine("BRAKE");
                        HandleDroneFlightMode(CustomMode.BRAKE);
                        return;
                    }
                }
            }
            
            Console.WriteLine("Arrival");
            
            HandleDroneFlightCommand(DroneFlightCommand.LAND);
        }
        catch (Exception e)
        {
            Console.WriteLine($"오류 발생: {e}");
        }


    }
    
    public async Task HandleDroneMoveToTarget()
    {
        _mapper.setControlStt("manual");
        
        double lat = _mapper.getTargetPointLat();
        List<DroneLocation> transit = _mapper.getTransitPoint();
        double lng = _mapper.getTargetPointLng();
        float alt = _mapper.getMissionAlt();
        int count = 0;
        
        // HandleDroneFlightMode(CustomMode.GUIDED);
        //
        // HandleDroneFlightCommand(DroneFlightCommand.ARM);
        // Thread.Sleep(1000);
        //
        // HandleDroneFlightCommand(DroneFlightCommand.TAKEOFF);
        
        double flightDistance = 0;
        _mapper.setCurrentDistance(0);
        
        if (transit.Count == 0)
        {
            flightDistance += _vincentyCalculator.DistanceCalculater(
                _mapper.getStartPointLat(),
                _mapper.getStartPointLng(),
                lat, lng);
            _mapper.setTotalDistance(flightDistance);
        }
        else
        {
            for (int i = 0; i < transit.Count; i++)
            {
                flightDistance += _vincentyCalculator.DistanceCalculater(
                    (i == 0) ? _mapper.getStartPointLat() : transit[i - 1].lat,
                    (i == 0) ? _mapper.getStartPointLng() : transit[i - 1].lng,
                    transit[i].lat, transit[i].lng
                );
            }
            flightDistance += _vincentyCalculator.DistanceCalculater(
                transit.Last().lat, transit.Last().lng, lat, lng
            );
            _mapper.setTotalDistance(flightDistance);
        }
        // Thread.Sleep(1000);
        //
        // while (_mapper.getRelativeAlt() < _mapper.getMissionAlt() - 0.1) { }
        // Console.WriteLine("Mission Alt Reached");
        
        foreach (DroneLocation location in transit)
        {
            count += 1;
            await sendMission(location.lat, location.lng, alt);
            Thread.Sleep(1000);
            while (_vincentyCalculator.DistanceCalculater(_mapper.getCurrentLat(), _mapper.getCurrentLon(),
                       location.lat, location.lng) > 1.5)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine("반복문 중단");
                        HandleDroneFlightMode(CustomMode.BRAKE);
                        return;
                    }
                }
                
            }
            Console.WriteLine($"Transit{count} Arrival");
        }
        await sendMission(lat, lng, alt);
        Thread.Sleep(1000);
        while (_vincentyCalculator.DistanceCalculater(_mapper.getCurrentLat(), _mapper.getCurrentLon(),
                   lat, lng) > 1.5)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("반복문 중단");
                    HandleDroneFlightMode(CustomMode.BRAKE);
                    return;
                }
            }
        }
        // Console.WriteLine("Arrival");
        // 
        // HandleDroneFlightCommand(DroneFlightCommand.LAND);
    }
    

    public async Task sendMission(double lat, double lng, float alt)
    {
        var commandBody = new MAVLink.mavlink_mission_item_t()
        {
            command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
            x = (float)lat,
            y = (float)lng,
            z = alt,            
            autocontinue = 1,   
            current = 2,        // 2로 해야 움직임 (이유 모르겠음..) 
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,      
            frame = 3,          // default: 0(위도경도고도 전역 좌표계), 3(위도경도 전역, 고도 상대좌표계)
            target_system = 1,
        };
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.MISSION_ITEM,
            commandBody));
        await SetCommandAsync(msg);
    }

    public async Task HandleDroneMoveToBase()
    {
        double lat = _mapper.getStartPointLat();
        double lng = _mapper.getStartPointLng();
        float alt = _mapper.getMissionAlt();
        
        var commandBody = new MAVLink.mavlink_mission_item_t()
        {
            command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
            // x = (int)Math.Round(lat * 10000000),
            // y = (int)Math.Round(lng * 10000000),
            x = (float)lat,
            y = (float)lng,
            z = alt,            
            autocontinue = 0,   
            current = 2,        // 현재 웨이포인트 번호
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,      
            frame = 3,          // default: 0(위도경도고도 전역 좌표계), 3(위도경도 전역, 고도 상대좌표계)
            target_system = 1,
        };
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.MISSION_ITEM,
            commandBody));
        await SetCommandAsync(msg);
    }
    
    /*
     * throttle(상승하강): 상승 (상) (상)   하강 (하) (하)
     *                      (상) (상)       (하) (하)
     *
     * roll(측방기동): 좌측 (하) (상)   우측 (상) (하)
     *                  (하) (상)       (상) (하)
     * 
     * pitch(전지후진): 전진 (하) (하)  후진 (상) (상)
     *                   (상) (상)      (하) (하)
     * 
     * yaw(좌우회전): 좌회전 (상) (하)   우회전 (하) (상)
     *                   (하) (상)        (상) (하)
     * 
     * chan1_raw: Roll
     * chan2_raw: Pitch
     * chan3_raw: Throttle
     * chan4_raw: Yaw
     * 
     */
    
    // 드론
    public async Task HandleDroneJoystick(ArrowButton arrow)
    {
        MAVLink.mavlink_rc_channels_override_t commandBody;
    
        switch (arrow)
        {
            case UP:    // drone up btn, throttle up
                // Console.WriteLine($"input Drone : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = (ushort)(1500 + ThrottleIncrement),
                    chan4_raw = 1500,
                };
                break;
            case DOWN:  // drone down btn, throttle down 
                // Console.WriteLine($"input Drone : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = (ushort)(1500 - ThrottleIncrement),
                    chan4_raw = 1500
                };
                break;
            case LEFT:  // drone left btn, yaw left
                // Console.WriteLine($"input Drone : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = 1500,
                    chan4_raw = (ushort)(1500 - yawIncrement),
                };
                break;
            case RIGHT: // drone right btn, yaw right
                // Console.WriteLine($"input Drone : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = 1500,
                    chan4_raw = (ushort)(1500 + yawIncrement),
                };
                break;
            default:
                commandBody = new MAVLink.mavlink_rc_channels_override_t();
                break;
        }
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
                    MAVLink.MAVLINK_MSG_ID.RC_CHANNELS_OVERRIDE, commandBody));
        await SetCommandAsync(msg);
    }

    // 제어 
    public async Task HandleControlJoystick(ArrowButton arrow)
    {
        MAVLink.mavlink_rc_channels_override_t commandBody;
        switch (arrow)
        {
            case UP:    // controll up btn, pitch up
                // Console.WriteLine($"input Controll : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = 1500,
                    chan2_raw = (ushort)(1500 - ThrottleIncrement),
                    chan3_raw = 1500,
                    chan4_raw = 1500
                };
                break;
            case DOWN:  // controll down btn, pitch down 
                // Console.WriteLine($"input Controll : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = 1500,
                    chan2_raw = (ushort)(1500 + ThrottleIncrement),
                    chan3_raw = 1500,
                    chan4_raw = 1500
                };
                break;
            case LEFT:  // controll left btn, roll left 
                // Console.WriteLine($"input Controll : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = (ushort)(1500 - ThrottleIncrement),
                    chan2_raw = 1500,
                    chan3_raw = 1500,
                    chan4_raw = 1500,
                };
                break;
            case RIGHT: // controll right btn, roll right
                // Console.WriteLine($"input Controll : {arrow}");
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    chan1_raw = (ushort)(1500 + ThrottleIncrement),
                    chan2_raw = 1500,
                    chan3_raw = 1500,
                    chan4_raw = 1500,
                };
                break;
            default:
                commandBody = new MAVLink.mavlink_rc_channels_override_t();
                break;
        }
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.RC_CHANNELS_OVERRIDE, commandBody));
        await SetCommandAsync(msg);
    }

    // 카메라 
    public async Task HandleCameraJoystick(ArrowButton arrow)
    {
        Console.WriteLine($"input Camera : {arrow}");
    }




    // 공용
    public async Task SetCommandAsync(MAVLink.MAVLinkMessage msg)
    {
        if (_context is null || !_context.Channel.Active) return;
     
        try {
            // await _context.Channel.WriteAndFlushAsync(EncodeUdpDroneMessage(msg));
            await _context.Channel.WriteAndFlushAsync(EncodeUdpDroneMessage(msg));
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }
    
    private DatagramPacket EncodeUdpDroneMessage(MAVLink.MAVLinkMessage msg)
    {
        // MAVLink 메시지를 MAVLink 2.0 패킷으로 인코딩하여 바이트 배열로 만듬 (Netty 라이브러리의 Unpooled.WrappedBuffer를 사용하여 바이트 배열을 Netty의 버퍼로 래핑)
        var encodeMsg = Unpooled.WrappedBuffer(_parser.GenerateMAVLinkPacket20(
            (MAVLink.MAVLINK_MSG_ID)msg.msgid,  
            msg.data,                           
            sign: false,
            msg.sysid,
            msg.compid,
            msg.seq));
        
        // 래핑된 버퍼와 드론 주소를 사용하여 새로운 DatagramPacket을 생성하고 반환, DatagramPacket은 네트워크 패킷을 나타내는 Netty 라이브러리의 클래스
        return new DatagramPacket(encodeMsg, _droneAddress);
    }
    
    public async Task DisconnectAsync()
    {
        if (_context == null) return;
        await _context.DisconnectAsync();
    }
}