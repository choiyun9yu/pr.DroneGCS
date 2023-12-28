using MAVSDK;

using kisa_gcs_system.Interfaces;

namespace kisa_gcs_system.Services;

/*
 * STX: 0xFD로 고정된 패킷 시작 마커
 * LEN: "PAYLOAD" 부분의 길이
 * INC FLAGS: MAVLINK 호환 플래그, 이해할 수 없는 플래그를 가진 패킷은 버려진다. 보통은 0x00 이다.
 * CMP FLAGS: MAVLINK 비호환 플래그, 이해할 수 없는 플래그를 가진 패킷도 처리된다. 비표준 구현체등에 사용도리 수 있다. 보통 0x00이다.
 * SEQ: 메세지의 시퀀스 번호
 * SYS ID: 송신자의 시스템 ID, 용도나 구현체에 따라서 임의로 지정
 * COMP ID: 송신자의 컴포넌트 ID, 용도나 구현체에 따라서 임의로 지정
 * MSG ID: 3바이트로 구성된 메시지 ID, 메시지의 의미 나타냄
 * PAYLOAD: 메세지의 실제 데이터 (최대 255 바이트)
 * CHECKSUM: 메시지의 CRC 체크섬
 * SIGNATURE: 메세지의 서명 (보통은 생략)
 */

public class DroneController
{
    private readonly MavlinkMapper _mapper = new();
    private readonly IHubContext<DroneHub> _hubContext;

    public DroneController(IHubContext<DroneHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task HandleMavlinkMessage(MAVLink.MAVLinkMessage msg, DroneCommunication link)
    {
        string droneId = msg.sysid.ToString();
        _mapper.SetDroneId(droneId);
        
        object data = msg.data;
        _mapper.PredictionMapping(data);
        _mapper.GcsMapping(data);
        
        string droneMessage = _mapper.ObjectToJson();
        await _hubContext.Clients.All.SendAsync("droneMessage", droneMessage);
        
    }
}