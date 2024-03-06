using System.Text.Json;
using MAVSDK;

using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Models;
using kisa_gcs_system.Models.Helper;
using kisa_gcs_system.Services.Helper;

using static kisa_gcs_system.Models.ArrowButton;

namespace kisa_gcs_system.Services;

public class DroneControlService : IDroneControlService
{
    private readonly IHubContext<DroneControlService> _hubContext;
    private IChannelHandlerContext? _context;
    
    private Dictionary<string, DroneState> _droneStateMap = new();
    private Dictionary<string, IPEndPoint> _droneAddressMap = new();
    private DroneState _droneInstance;
    private string _selectedDrone;
    private string _droneId;
    private IPEndPoint _droneAddress;
    
    private readonly MAVLink.MavlinkParse _parser = new();
    private readonly MavlinkMapper _mapper = new();
    
    private readonly GcsApiService _gcsApiService;
    private IMongoCollection<Dashboard> _dashboard;

    private VincentyCalculator _vincentyCalculator = new();
    
    private static int ThrottleIncrement = 300;
    private static int yawIncrement = 50;
    
    public DroneControlService(IConfiguration configuration, IHubContext<DroneControlService> hubContext, GcsApiService gcsApiService)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _gcsApiService = gcsApiService;
        
        var connectionString = configuration.GetConnectionString("MongoDB");
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("drone");
        _dashboard = database.GetCollection<Dashboard>("dashboard_info");
    }

    // 클라이언트가 연결될 때, 연결 ID를 기록해두는 메서드 ( 클라이언트가 연결될 때마다 수행 ) 웹은 새로고침할때마다 ConnectionId 바뀜 
    public override async Task<Task> OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        
        await Groups.AddToGroupAsync(connectionId, "Python");
        
        return base.OnConnectedAsync();
    }
    
    // 클라이언트에서 타입 받기
    public async Task SendClientType()
    {
        var connectionId = Context.ConnectionId;
        
        // 새로 연결되면 모두 Python 그룹에 우선 포함시키고, 리액트 연결인 경우 파이썬 그룹에서 제거하고 리액트 그룹으로 재등록 하는 방법 사용
        await Groups.RemoveFromGroupAsync(connectionId, "Python");
        
        await Groups.AddToGroupAsync(connectionId, "React");
    }
    
    // 드론 목록 내보내기
    public override async Task GetDroneList()
    {
        string[] droneids = _droneStateMap.Keys.ToArray();
        string json = JsonConvert.SerializeObject(droneids);
        // Console.WriteLine(json);
        await _hubContext.Clients.Group("React").SendAsync("droneList", json);
    }
    
    // 드론 선택하기
    public override async Task SelectDrone(string selectedDroneId)
    {
        try
        {
            _selectedDrone = selectedDroneId;
            _droneInstance = _droneStateMap[selectedDroneId];
            _droneAddress = _droneAddressMap[selectedDroneId];
            Console.WriteLine($"Selected Drone ID: {selectedDroneId}");
            Console.WriteLine($"Selected Drone Address: {_droneAddressMap[selectedDroneId]}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    // 드론 상태 정보 내보내기 
    public override async Task HandleMavlinkMessage(IChannelHandlerContext ctx, MAVLink.MAVLinkMessage msg)
    {
        _context = ctx; 
        _droneId = msg.sysid.ToString();
        
        if (!_droneStateMap.ContainsKey(_droneId))
        {
            _droneStateMap[_droneId] = new DroneState(_droneId);
            _droneAddressMap[_droneId] = GetChannelEndpoint(_context);
            Console.WriteLine($"New Drone {_droneId} Added, {GetChannelEndpoint(_context)}");
        }
        
        if (_selectedDrone is null) _selectedDrone = _droneId;
        
        if (_droneAddress is null) _droneAddress = _droneAddressMap[_selectedDrone];
        
        if ((MAVLink.MAVLINK_MSG_ID)msg.msgid == MAVLink.MAVLINK_MSG_ID.STATUSTEXT)
        {
            var logdata = (MAVLink.mavlink_statustext_t)msg.data;
            var text = string.Join("", logdata.text.Select(c => (char)c));
            
            if (text.StartsWith("Disarming motors")) CompleteMission(_droneId);

            _droneStateMap[_droneId].DroneStt.DroneLogger.Add(
                new MavlinkLog
                {
                    logtime = DateTime.Now,
                    message = text
                }
            );
        }
        
        object data = msg.data;
        _mapper.UpdateDroneState(_droneStateMap[_droneId], data);
        
        string forReact = JsonConvert.SerializeObject(_droneStateMap[_selectedDrone]);
        await _hubContext.Clients.Group("React").SendAsync("droneState", forReact);

        object obj = new
        {
            DroneId = _droneId,
            FlightId = _droneStateMap[_droneId].FlightId,
            SensorData =_droneStateMap[_droneId].SensorData 
        };
        string forPython = JsonConvert.SerializeObject(obj);
        await _hubContext.Clients.Group("Python").SendAsync("droneState", forPython);
    }
    
    // IChannelHandlerContext에서 엔드포인트 정보를 가져오는 메서드, IPEndPoint 클래스는 IP 주소와 포트 번호를 나타낸다. 주로 소켓 프로그래밍에서 사용되며 특히 TCP 또는 UDP 통신에서 호스트와 포트를 식별하는데 유용하다.   
    private IPEndPoint GetChannelEndpoint(IChannelHandlerContext ctx)
    {
        // 현재 채널의 원격 주소를 IPEndPoint로 캐스팅해서 정보 가져오기
        var contextEp = (IPEndPoint)ctx.Channel.RemoteAddress;
        
        // 채널의 속성에서 "SenderAddress" 라는 키로 저장된 IPEndPoint 정보 가져오기
        return contextEp ?? ctx.Channel.GetAttribute(AttributeKey<IPEndPoint>.ValueOf("SenderAddress")).Get()!;
    }
    
    private void UpdateDroneAddress(IChannelHandlerContext ctx)
    {
        var senderEp = GetChannelEndpoint(ctx);

        if (_droneAddress == null)
        {
            // 새로운 IPEndPoint를 생성하여 드론의 주소로 설정 
            _droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
        }
        else
        {
            // 스레드 안전하게 업데이트 (여러 스레드에서 동시에 업데이트가 발생할 수 있는 상황에서의 경합 조건을 방지)
            lock (_droneAddress)
            {
                _droneAddress = new IPEndPoint(senderEp.Address.MapToIPv4(), senderEp.Port);
            }
        }
    }
    
    public void UpdateDroneLogger(string text)
    {
        _droneInstance.DroneStt.DroneLogger.Add(
            new MavlinkLog()
            {
                logtime = DateTime.Now,
                message = text
            }
        );
    }
    

    // 비행 모드 변경 메소드 (Auto ~ RTL)
    public override async Task HandleDroneFlightMode(CustomMode flightMode)
    {
        // MAVLink 프로토콜에서 사용되는 메시지 및 명령 생성
        var commandBody = new MAVLink.mavlink_set_mode_t()
        { 
            // 시뮬레이터는 SYS ID 가 1 이어서?
            target_system = byte.Parse(_selectedDrone),
            custom_mode = (uint)flightMode,
            base_mode = 1,
        };
         
        // 생성된 명령을 이용하여 MAVLink 메시지 생성 
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.SET_MODE, commandBody));
         
        // 생성된 MAVLink 메시지를 이용하여 드론에 비행 모드 변경 명령을 비동기적으로 전송 
        await SendCommandAsync(msg);
     }

    // 비행 명령 메소드 (Arm ~ Land)
    public override async Task HandleDroneFlightCommand(DroneFlightCommand flightCommand)
    {
        try
        {
            MAVLink.mavlink_command_long_t? commandBody = null;
            MAVLink.mavlink_set_mode_t? setModeMsg = null;

            switch (flightCommand)
            {
                case DroneFlightCommand.ARM:
                {
                    commandBody = new MAVLink.mavlink_command_long_t
                    {
                        target_system = byte.Parse(_selectedDrone),
                        command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM,
                        param1 = 1 // arm
                    };
                    break;
                }
                
                case DroneFlightCommand.DISARM:
                {
                    commandBody = new MAVLink.mavlink_command_long_t()
                    {
                        target_system = byte.Parse(_selectedDrone),
                        command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM,
                        param1 = 0,     // dis-arm
                        param2 = 21196  // 펌웨어에 따라 다른 값일 수 있음 
                    };
                    break;
                }
                
                case DroneFlightCommand.TAKEOFF:
                {
                    int alt = _droneInstance.DroneMission.MissionAlt;
                    if (_droneInstance.DroneStt.Alt < 0.5 && _droneInstance.DroneStt.FlightMode == CustomMode.GUIDED)
                    {
                        StartMission();
                        commandBody = new MAVLink.mavlink_command_long_t
                        {
                            target_system = byte.Parse(_selectedDrone),
                            command = (ushort)MAVLink.MAV_CMD.TAKEOFF,
                            param7 = alt,       // z(m), 드론의 이륙 높이(미터) 
                            param3 = (float)5,  // ascend rate (m/s), 이륙 중에 드론이 수직으로 상승하는 속도
                            param1 = 0,         // pitch(rad), 드론의 전방 기울기 각도 
                            param4 = 0,         // yaw(rad), 드론의 회전을 나타내는 각도
                            param5 = 0,         // x, 드론의 이륙 위치 x
                            param6 = 0,         // y, 드론의 이륙 위치 y
                        };
                    }
                    break;
                }
                
                case DroneFlightCommand.LAND:
                {
                    setModeMsg = new MAVLink.mavlink_set_mode_t()
                    {
                        target_system = byte.Parse(_selectedDrone),
                        base_mode = 1,
                        custom_mode = (byte)CustomMode.LAND,
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
            
            await SendCommandAsync(msg); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling drone flight command '{flightCommand}': {ex.Message}");
        }
    }

    // 미션 부여하기 
    private void StartMission()
    {
        DateTime current = DateTime.Now;
        _droneInstance.IsLanded = false;
        _droneInstance.DroneMission.StartTime = current;
        _droneInstance.DroneMission.CompleteTime = null;
        _droneInstance.DroneMission.DroneTrails = new FixedSizedQueue<DroneLocation>(600);
        string flightId = $"{current.Year}{current.Month:D2}{current.Day:D2}{current.Hour:D2}{current.Minute:D2}{current.Second:D2}";
        _droneInstance.FlightId = $"{flightId}t";
        Console.WriteLine($"Start : {flightId}");
    }

    private void CompleteMission(string droneId)
    {
        _droneInstance.DroneMission.CompleteTime = DateTime.Now;
        _droneInstance.IsLanded = true;
        _droneInstance.FlightId = "None";
        
        var dashboard = new Dashboard
        {
            DroneId = droneId,
            StartTime = _droneStateMap[droneId].DroneMission.StartTime,
            CompleteTime = _droneStateMap[droneId].DroneMission.CompleteTime,
            StartPoint = _droneStateMap[droneId].DroneMission.StartPoint,
            TransitPoints = _droneStateMap[droneId].DroneMission.TransitPoint,
            LandPoint = new DroneLocation
            {
                lat = _droneStateMap[droneId].DroneStt.Lat,
                lng = _droneStateMap[droneId].DroneStt.Lon
            },
            FlightDistance = _droneStateMap[droneId].DroneMission.TotalDistance
        };
        
        try
        {
            _dashboard.InsertOne(dashboard);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public async Task HandleDroneStartMarking(double lat, double lng)
    {
        _droneInstance.DroneMission.StartPoint = new DroneLocation{
            lat = lat,
            lng = lng
        };
    }

    public async Task HandleDroneTargetMarking(double lat, double lng)
    {
        _droneInstance.DroneMission.TargetPoint = new DroneLocation
        {
            lat = lat,
            lng = lng
        };

        _droneInstance.DroneMission.TotalDistance = _vincentyCalculator.DistanceCalculater(
            _droneInstance.DroneMission.StartPoint.lat, _droneInstance.DroneMission.StartPoint.lng, lat, lng);
    }

    public async Task HandleDroneTransitMarking(object transitList)
    {
        if (transitList is JsonElement jsonElement)
        {
            List<DroneLocation> transitPoints = new List<DroneLocation>();
            
            foreach (JsonElement arrayElement in jsonElement.EnumerateArray())
            {
                DroneLocation location = ParseDroneLocation(arrayElement);
                transitPoints.Add(location);
            }

            _droneInstance.DroneMission.TransitPoint = transitPoints;
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
        _droneInstance.DroneMission.MissionAlt = missionalt;
    }
    
    public async Task sendMission(double lat, double lng, float alt)
    {
        /*
         * mavlink_mission_item_t 는 자동 비행 임무 또는 미션을 정의하는데 사용된다.
         * 이 메시지는 단일 미션 항목에 대한 정보를 포함하고 있으며, 드론이 수행해야할 다음 작업을 지정한다.
         * 
         * seq: 미션 항목의 시퀀스 번호 
         * frame: 미션 항목의 좌표 프레임 기준 설정 default: 0(위도경도고도 전역 좌표계), 3(위도경도 전역, 고도 상대좌표계)
         * command: 수행 해야할 명령 (이륙, 창륙, 경로 따라가기 등)
         * param1, param2, param3, param4:
         * autocontinue: 미션 항목이 완료된 후 다음 미션 항목으로 자동으로 계속할지 여부를 나타냄
         * current: 현재 활성화된 미션 항목인지 여부를 나타냄 (이 필드가 1이면 해당 미션항목이 활성화된 항목이고, 0이면 비활성화된 항목)
         * misstion_type: 미션의 종류를 나타냄 (Mission: 주요 미션에 대한 미션 명령, 주로 비행 경로 정의, FENCE: 드론 비행 경계 설정, RALLY: 차량의 랠리포인트 지정, ALL: 모든 미션 유형을 지우는데 사용)
         */
        
        var commandBody = new MAVLink.mavlink_mission_item_t
        {
            target_system = byte.Parse(_selectedDrone),
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,      
            frame = 3,         
            command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
            autocontinue = 1,   
            current = 2,        // 2로 해야 움직임 (이유 모르겠음..)
            x = (float)lat,
            y = (float)lng,
            z = alt,           
        };
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.MISSION_ITEM,
            commandBody));
        await SendCommandAsync(msg);
    }
    
    public async Task HandleDroneMoveToTarget()
    {
        _droneInstance.ControlStt = "manual";

        double lat = _droneInstance.DroneMission.TargetPoint.lat;
        List<DroneLocation> transit = _droneInstance.DroneMission.TransitPoint;
        double lng =_droneInstance.DroneMission.TargetPoint.lng;
        float alt = _droneInstance.DroneMission.MissionAlt;
        int count = 0;
        
        double flightDistance = 0;
        _droneInstance.DroneMission.CurrentDistance = 0;
        
        if (transit.Count == 0)
        {
            flightDistance += _vincentyCalculator.DistanceCalculater(
                _droneInstance.DroneMission.StartPoint.lat,
                _droneInstance.DroneMission.StartPoint.lng,
                lat, lng);
            _droneInstance.DroneMission.TotalDistance = flightDistance ;
        }
        else
        {
            for (int i = 0; i < transit.Count; i++)
            {
                flightDistance += _vincentyCalculator.DistanceCalculater(
                    (i == 0) ? _droneInstance.DroneMission.StartPoint.lat : transit[i - 1].lat,
                    (i == 0) ?_droneInstance.DroneMission.StartPoint.lng : transit[i - 1].lng,
                    transit[i].lat, transit[i].lng
                );
            }
            flightDistance += _vincentyCalculator.DistanceCalculater(
                transit.Last().lat, transit.Last().lng, lat, lng
            );
            _droneInstance.DroneMission.TotalDistance = flightDistance;
        }
        
        foreach (DroneLocation location in transit)
        {
            count += 1;
            await sendMission(location.lat, location.lng, alt);
            Thread.Sleep(1000);
            while (_vincentyCalculator.DistanceCalculater(_droneInstance.DroneStt.Lat, _droneInstance.DroneStt.Lon,
                       location.lat, location.lng) > 1.5)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine("Loop Break");
                        HandleDroneFlightMode(CustomMode.BRAKE);
                        return;
                    }
                }
                
            }
            Console.WriteLine($"Transit{count} Arrival");
        }
        await sendMission(lat, lng, alt);
        Thread.Sleep(1000);
        while (_vincentyCalculator.DistanceCalculater(_droneInstance.DroneStt.Lat, _droneInstance.DroneStt.Lon,
                   lat, lng) > 1.5)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Loop Break");
                    HandleDroneFlightMode(CustomMode.BRAKE);
                    return;
                }
            }
        }
    }

    // public async Task HandleDroneMoveToMission(string startPoint, string targetPoint, List<string> transitPoint, int alt, string totalDistance)
    // {
    //     Console.WriteLine("ARM!!!!");
    //     MAVLink.mavlink_command_long_t? armCommand = new MAVLink.mavlink_command_long_t
    //     {
    //         target_system = byte.Parse(_selectedDrone),
    //         command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM,
    //         param1 = 1 // arm
    //     };
    //     var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
    //         MAVLink.MAVLINK_MSG_ID.COMMAND_LONG,
    //         armCommand));
    //     await SendCommandAsync(msg); 
    //     Thread.Sleep(1000);
    //     
    //     Console.WriteLine("Take Off!!!!");
    //     MAVLink.mavlink_command_long_t? takeoffCommand = new MAVLink.mavlink_command_long_t
    //     {
    //         target_system = byte.Parse(_selectedDrone),
    //         command = (ushort)MAVLink.MAV_CMD.TAKEOFF,
    //         param7 = 10,        // z(m), 드론의 이륙 높이(미터) 
    //         param3 = 5,         // ascend rate (m/s), 이륙 중에 드론이 수직으로 상승하는 속도
    //         param1 = 0,         // pitch(rad), 드론의 전방 기울기 각도 
    //         param4 = 0,         // yaw(rad), 드론의 회전을 나타내는 각도
    //         param5 = 0,         // x, 드론의 이륙 위치 x
    //         param6 = 0,         // y, 드론의 이륙 위치 y
    //     };
    //     var msg1 = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
    //         MAVLink.MAVLINK_MSG_ID.COMMAND_LONG,
    //         takeoffCommand));
    //     await SendCommandAsync(msg1); 
    //     Thread.Sleep(5000);
    //     
    //     Console.WriteLine("A Point!!!!");
    //     MAVLink.mavlink_mission_item_t? commandBodyA = new MAVLink.mavlink_mission_item_t
    //     {
    //         seq = 0,
    //         target_system = byte.Parse(_selectedDrone),
    //         mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,      
    //         frame = 3,         
    //         command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
    //         current = 2,        // 2로 해야 움직임 (이유 모르겠음..)
    //         x = (float)36.38324551,
    //         y = (float)127.36622289,
    //         z = 10,           
    //         autocontinue = 1,   
    //     };
    //     var msg2 = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
    //         MAVLink.MAVLINK_MSG_ID.MISSION_ITEM,
    //         commandBodyA));
    //     await SendCommandAsync(msg2);
    //     await Task.Delay(2000);
    //
    //     Console.WriteLine("B Point!!!!");
    //     MAVLink.mavlink_mission_item_t? commandBodyB = new MAVLink.mavlink_mission_item_t
    //     {
    //         seq = 1,
    //         target_system = byte.Parse(_selectedDrone),
    //         mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,      
    //         frame = 3,         
    //         command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
    //         current = 2,        // 2로 해야 움직임 (이유 모르겠음..)
    //         x = (float)36.38654494,
    //         y = (float)127.37244562,
    //         z = 10,           
    //         autocontinue = 1,   
    //     };
    //     var msg3 = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
    //         MAVLink.MAVLINK_MSG_ID.MISSION_ITEM,
    //         commandBodyB));
    //     await SendCommandAsync(msg3);
    //
    //     Console.WriteLine("LAND!!!!");
    //     MAVLink.mavlink_set_mode_t landCommand = new MAVLink.mavlink_set_mode_t()
    //     {
    //         target_system = byte.Parse(_selectedDrone),
    //         base_mode = 1,
    //         custom_mode = (byte)CustomMode.LAND,       
    //     };
    //     var msg4 = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
    //         MAVLink.MAVLINK_MSG_ID.SET_MODE,
    //         landCommand));
    //     await SendCommandAsync(msg4);
    // }


    public async Task HandleDroneMoveToMission(string startPoint, string targetPoint, List<string> transitPoint, int alt, string totalDistance)
    {
        try
        {
            List<double>? StartPoint = _gcsApiService.GetLocalPoint(startPoint);
            if (_vincentyCalculator.DistanceCalculater(_droneInstance.DroneStt.Lat, _droneInstance.DroneStt.Lon,
                    StartPoint[0], StartPoint[1]) > 1.5)
            {
                throw new Exception("Wrong Starting Point");
            }
    
            string trimmedTotalDistance = totalDistance.Substring(0, totalDistance.Length - 3);
            string numericPart = new string(trimmedTotalDistance.Where(char.IsDigit).ToArray());
            double TotalDistance = double.Parse(numericPart);
    
            _droneInstance.ControlStt = "auto";
            _droneInstance.DroneMission.CurrentDistance = 0;
            _droneInstance.DroneMission.MissionAlt = alt;
            _droneInstance.DroneMission.TotalDistance = TotalDistance;
            _droneInstance.DroneMission.TransitPoint = new List<DroneLocation>();
            
            HandleDroneFlightMode(CustomMode.GUIDED);
            
            HandleDroneFlightCommand(DroneFlightCommand.ARM);
            Thread.Sleep(1000);
            
            HandleDroneFlightCommand(DroneFlightCommand.TAKEOFF);
            Thread.Sleep(1000);
            while ((double)_droneInstance.DroneStt.Alt < _droneInstance.DroneMission.MissionAlt - 0.1) { }
            Console.WriteLine("Mission Alt Reached");
    
            for (int i = 0; i < transitPoint.Count; i++)
            {
                List<double>? TransitPoint = _gcsApiService.GetLocalPoint(transitPoint[i]);
                await sendMission(TransitPoint[0], TransitPoint[1], alt);
                _droneInstance.DroneMission.PathIndex = i + 1;
    
                _droneInstance.DroneMission.TransitPoint.Add(
                    new DroneLocation
                        {
                            lat = TransitPoint[0],
                            lng = TransitPoint[1]
                        });
                
                Thread.Sleep(1000);
                while (_vincentyCalculator.DistanceCalculater(_droneInstance.DroneStt.Lat, _droneInstance.DroneStt.Lon,
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
            _droneInstance.DroneMission.PathIndex = transitPoint.Count + 1;
            Thread.Sleep(1000);
            while (_vincentyCalculator.DistanceCalculater(_droneInstance.DroneStt.Lat, _droneInstance.DroneStt.Lon,
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
            Console.WriteLine($"Error: {e}");
        }
        
    }

    public async Task HandleDroneMoveToBase()
    {
        double lat = _droneInstance.DroneMission.StartPoint.lat;
        double lng = _droneInstance.DroneMission.StartPoint.lng;
        float alt = _droneInstance.DroneMission.MissionAlt;
        
        var commandBody = new MAVLink.mavlink_mission_item_t()
        {
            target_system = byte.Parse(_selectedDrone),
            command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
            x = (float)lat,
            y = (float)lng,
            z = alt,            
            autocontinue = 0,   
            current = 2,        // 현재 웨이포인트 번호
            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION,      
            frame = 3,          // default: 0(위도경도고도 전역 좌표계), 3(위도경도 전역, 고도 상대좌표계)

        };
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.MISSION_ITEM,
            commandBody));
        await SendCommandAsync(msg);
    }
    
    // 드론
    public async Task HandleDroneJoystick(ArrowButton arrow)
    {
        MAVLink.mavlink_rc_channels_override_t commandBody;
    
        switch (arrow)
        {
            case ArrowButton.UP:    
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = (ushort)(1500 + ThrottleIncrement),
                    chan4_raw = 1500,
                };
                break;
            case ArrowButton.DOWN:  
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = (ushort)(1500 - ThrottleIncrement),
                    chan4_raw = 1500
                };
                break;
            case ArrowButton.LEFT: 
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    chan1_raw = 1500,
                    chan2_raw = 1500,
                    chan3_raw = 1500,
                    chan4_raw = (ushort)(1500 - yawIncrement),
                };
                break;
            case ArrowButton.RIGHT: 
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
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
        await SendCommandAsync(msg);
    }

    // 제어 
    public async Task HandleControlJoystick(ArrowButton arrow)
    {
        MAVLink.mavlink_rc_channels_override_t commandBody;
        switch (arrow)
        {
            case ArrowButton.UP:   
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    chan1_raw = 1500,
                    chan2_raw = (ushort)(1500 - ThrottleIncrement),
                    chan3_raw = 1500,
                    chan4_raw = 1500
                };
                break;
            case ArrowButton.DOWN: 
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    chan1_raw = 1500,
                    chan2_raw = (ushort)(1500 + ThrottleIncrement),
                    chan3_raw = 1500,
                    chan4_raw = 1500
                };
                break;
            case ArrowButton.LEFT:  
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    chan1_raw = (ushort)(1500 - ThrottleIncrement),
                    chan2_raw = 1500,
                    chan3_raw = 1500,
                    chan4_raw = 1500,
                };
                break;
            case ArrowButton.RIGHT: 
                commandBody = new MAVLink.mavlink_rc_channels_override_t()
                {
                    target_system = byte.Parse(_selectedDrone),
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
        await SendCommandAsync(msg);
    }

    
    // 카메라 
    public async Task HandleCameraJoystick(ArrowButton arrow)
    {
        Console.WriteLine($"input Camera : {arrow}");
        MAVLink.mavlink_gimbal_manager_set_pitchyaw_t commandBody;
        
        switch (arrow)
        {
            case ArrowButton.UP:   
                commandBody = new MAVLink.mavlink_gimbal_manager_set_pitchyaw_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    target_component = 1,
                    gimbal_device_id = 1,
                    flags = 0,
                    pitch = 0.5f,               // 위아래 끄덕끄덕
                    yaw = 0,                    // 좌우 도리도리 
                    // pitch_rate = pitch_rate, // 위아래 회전 속도 
                    // yaw_rate = yaw_rate,     // 좌우 회전 속도 
                };
                break;
            case ArrowButton.DOWN: 
                commandBody = new MAVLink.mavlink_gimbal_manager_set_pitchyaw_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    target_component = 1,
                    gimbal_device_id = 1,
                    flags = 0,
                    pitch = 0.5f,               // 위아래 끄덕끄덕
                    yaw = 0,                    // 좌우 도리도리 
                    // pitch_rate = pitch_rate, // 위아래 회전 속도 
                    // yaw_rate = yaw_rate,     // 좌우 회전 속도 
                };
                break;
            case ArrowButton.LEFT:  
                commandBody = new MAVLink.mavlink_gimbal_manager_set_pitchyaw_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    target_component = 1,
                    gimbal_device_id = 1,
                    flags = 0,
                    pitch = 0,                   // 위아래 끄덕끄덕
                    yaw = 0.5f,                  // 좌우 도리도리 
                    // pitch_rate = pitch_rate,  // 위아래 회전 속도 
                    // yaw_rate = yaw_rate,      // 좌우 회전 속도 
                };
                break;
            case ArrowButton.RIGHT: 
                commandBody = new MAVLink.mavlink_gimbal_manager_set_pitchyaw_t()
                {
                    target_system = byte.Parse(_selectedDrone),
                    target_component = 1,
                    gimbal_device_id = 1,
                    flags = 0,
                    pitch = 0,            
                    yaw = 0.5f,    
                    // pitch_rate = pitch_rate, // 위아래 회전 속도 
                    // yaw_rate = yaw_rate,     // 좌우 회전 속도 
                };
                break;
            default:
                commandBody = new MAVLink.mavlink_gimbal_manager_set_pitchyaw_t();
                break;
        }
        var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
            MAVLink.MAVLINK_MSG_ID.GIMBAL_MANAGER_SET_PITCHYAW, commandBody));
        await SendCommandAsync(msg);
    }

    
    // Encoding
    public async Task SendCommandAsync(MAVLink.MAVLinkMessage msg)
    {
        // _context가 null 이거나 Channel이 활성화되지 않았을 경우 드론 통신을 위한 적절한 환경이 설정되어 있지 않다고 판단하고 메소드 중단
        if (_context is null || !_context.Channel.Active) return;
     
        try 
        {
            // WriteAndFlushAsync 메서드는 비동기적으로 작동하며, 메시지를 채널에 기록하고 즉시 채널을 플러시하여 즉시 전송한다.
            await _context.Channel.WriteAndFlushAsync(EncodeUdpDroneMessage(msg));
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }
    
    private DatagramPacket EncodeUdpDroneMessage(MAVLink.MAVLinkMessage msg)
    {
        if (_droneAddress != null)
        {
            // MAVLink 메시지를 MAVLink 2.0 패킷으로 인코딩하여 바이트 배열로 만듬 (Netty 라이브러리의 Unpooled.WrappedBuffer를 사용하여 바이트 배열을 Netty의 버퍼로 래핑)
            var encodeMsg = Unpooled.WrappedBuffer(_parser.GenerateMAVLinkPacket20(
                (MAVLink.MAVLINK_MSG_ID)
                msg.msgid,
                msg.data,
                sign: false,
                msg.sysid,
                msg.compid,
                msg.seq)
            );

            // 래핑된 버퍼와 드론 주소를 사용하여 새로운 DatagramPacket을 생성하고 반환, DatagramPacket은 네트워크 패킷을 나타내는 Netty 라이브러리의 클래스
            return new DatagramPacket(encodeMsg, _droneAddress);
        }
        throw new InvalidOperationException("_droneAddress is null.");
    }
    
    public async Task DisconnectAsync()
    {
        if (_context == null) return;
        await _context.DisconnectAsync();
    }
    
}

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

/* All 대신에 그룹으로 묶어서 특정 클라이언트에게 전송 가능
 * 특정 클라이언트에게 메시지를 보내기 위해서는 해당 클라이언트의 연결 ID를 사용하여 메시지를 보내야 한다.
 * SignalR은 연결 ID를 통해 각 클라이언트를 구분한다.
 * 따라서 특정 클라이언트에게 메시지를 보내려면 그 클라잉너트의 연결 ID를 알아야 한다.
 *
 * SignalR에서는 클라이언트가 연결되면 해당 클라이언트의 연결 ID를 알려주는 메커니즘이 있다.
 * 일반적으로 클라이언트가 연결되면 연결 ID를 서버로 보내고 서버에서는 해당 연결 ID를 기록해준다.
 * (웹 앱은 새로고침하면 그때마다 새로운 연결 ID가 할당된다.)
 */
        
// 특정 그룹에게 메시지 보내는 메서드 
// public async Task SendMessageToClientGroup(string clientType, string message)
// {
//     // 해당 클라이언트 종류의 그룹에 속한 모든 클라이언트에게 메시지 보내기 
//     await _hubContext.Clients.Group(clientType).SendAsync("sendMessage", message);
// }
    
// 특정 클라이언트에게 메시지를 보내는 메서드
// public async Task SendMessageToClient(string connectionId, string message)
// {
//     // connectionId 유효성 검사 추가 필요
//     if (string.IsNullOrEmpty(connectionId))
//     {
//         throw new ArgumentNullException(nameof(connectionId));
//     }
//     await _hubContext.Clients.Client(connectionId).SendAsync("droneMessage", message);
// }