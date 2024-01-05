using MAVSDK;

using kisa_gcs_system.Interfaces;

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

public class DroneController(IHubContext<DroneHub> hubContext)
{
    private readonly MAVLink.MavlinkParse _parser = new();
    private readonly MavlinkMapper _mapper = new();
    private readonly IHubContext<DroneHub> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    
    public async Task HandleMavlinkMessage(MAVLink.MAVLinkMessage msg, DroneCommunication link)
    {
        string droneId = msg.sysid.ToString();
        _mapper.SetDroneId(droneId);
        
        if ((MAVLink.MAVLINK_MSG_ID)msg.msgid == MAVLink.MAVLINK_MSG_ID.STATUSTEXT)
        {
            var logdata = (MAVLink.mavlink_statustext_t)msg.data;
            var text = string.Join("", logdata.text.Select(c => (char)c));
            _mapper.UpdateDroneLogger(text);
        }
        
        object data = msg.data;
        _mapper.PredictionMapping(data);
        _mapper.GcsMapping(data);
        
        string droneMessage = _mapper.ObjectToJson();
        await _hubContext.Clients.All.SendAsync("droneMessage", droneMessage);
    }
    
    // public async Task HandleDroneFlightMode(CustomMode flightMode)
    // {
    //     Console.WriteLine("Acting HandleDroneFlightMode");
    //     // MAVLink 프로토콜에서 사용되는 메시지 및 명령 생성
    //     var commandBody = new MAVLink.mavlink_set_mode_t()
    //     {
    //         // 시뮬레이터는 SYS ID 가 1 이어서? 
    //         target_system = (byte)1,
    //         custom_mode = (uint)flightMode,
    //         base_mode = 1,
    //     };
    //     
    //     // 생성된 명령을 이용하여 MAVLink 메시지 생성 
    //     var msg = new MAVLink.MAVLinkMessage(_parser.GenerateMAVLinkPacket20(
    //         MAVLink.MAVLINK_MSG_ID.SET_MODE, commandBody));
    //     
    //     // 생성된 MAVLink 메시지를 이용하여 드론에 비행 모드 변경 명령을 비동기적으로 전송 
    //     await SetCustomModeAsync(msg);
    // }
    //
    // private async Task SetCustomModeAsync(MAVLink.MAVLinkMessage msg)
    // {
    //     // 컨텍스트가 null 이거나 채널이 활성화되어 있지 않은 경우에는 메시지를 전송하지 않고 종료
    //     if (_context is null || !_context.Channel.Active)
    //     {
    //         return;
    //     }
    //
    //     try
    //     {
    //         await _context.Channel.WriteAndFlushAsync(this.EncodeUdpDroneMessage(msg));
    //     } catch (Exception e) {
    //         Console.WriteLine(e.Message);
    //     }
    // }
    //
    // private DatagramPacket EncodeUdpDroneMessage(MAVLink.MAVLinkMessage msg)
    // {
    //     // MAVLink 메시지를 MAVLink 2.0 패킷으로 인코딩하여 바이트 배열로 만듬 (Netty 라이브러리의 Unpooled.WrappedBuffer를 사용하여 바이트 배열을 Netty의 버퍼로 래핑)
    //     var encodeMsg = Unpooled.WrappedBuffer(_parser.GenerateMAVLinkPacket20(
    //         (MAVLink.MAVLINK_MSG_ID)msg.msgid,  
    //         msg.data,                           
    //         sign: false,
    //         msg.sysid,
    //         msg.compid,
    //         msg.seq
    //     ));
    //     // 래핑된 버퍼와 드론 주소를 사용하여 새로운 DatagramPacket을 생성하고 반환, DatagramPacket은 네트워크 패킷을 나타내는 Netty 라이브러리의 클래스
    //     return new DatagramPacket(encodeMsg, _droneAddress);
    // }
}