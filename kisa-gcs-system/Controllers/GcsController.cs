using MAVSDK;

using kisa_gcs_service.Model;
using kisa_gcs_service.Service;
using SignalR.Hubs;

namespace kisa_gcs_service.Controller;


public interface IGcsControllerUnit
{
    Task SendMessageAsync(object msg);
    Task DisconnectAsync();
}

public class GcsController : Hub<IDroneHubClient>
{
    private readonly Dictionary<string, DroneState> _droneStateMap = new();
    private readonly MAVLink.MavlinkParse _mavParser = new();
    private readonly Dictionary<DroneCommunication, MavlinkNetty> _udpCiConnections = new();

    public async Task HandleMavlinkMessage(MAVLink.MAVLinkMessage msg, IGcsControllerUnit controllerUnit, DroneCommunication link)
    {
        var droneId = msg.sysid.ToString();
        switch (msg.msgid)
        {
            case (byte)MAVLink.MAVLINK_MSG_ID.HEARTBEAT:
            {
                await HandleHeartbeat(msg, controllerUnit, link);
                break;
            }
        }
    }

    private async Task HandleHeartbeat(MAVLink.MAVLinkMessage msg, IGcsControllerUnit controllerUnit,
        DroneCommunication link)
    {
        // 이미 연결된 드론의 통신 정보가 있다면 제거
        if (_udpCiConnections.ContainsKey(link))
        {
            _udpCiConnections.Remove(link);
        }
        
        // HEARTBEAT 메시지의 데이터 추출
        var droneInfoData = (MAVLink.mavlink_heartbeat_t)(msg.data);
        
        // 무인 항공기(드론) 컴포넌트가 아닌 경우 무시
        if (droneInfoData.type is > 25 or 06)
        {
            return;
        }
        
        // 드론 시스템 ID를 문자열로 변환
        var droneId = msg.sysid.ToString();
        
        // 이미 등록된 드론인지 확인
        var isExistingDrone = _droneStateMap.ContainsKey(msg.sysid.ToString())
            ? _droneStateMap[msg.sysid.ToString()]
            : null;
        
        
        if (isExistingDrone != null) // 기존에 등록된 드론인 경우
        {
            // 상태 정보 업데이트
            isExistingDrone.LastHeartbeatMessage = DateTime.Now;
            isExistingDrone.IsOnline = true;
            isExistingDrone.CommunicationLink = link;
        }
        else // 새로운 드론인 경우
        {
            var msseage = new DRONE_NET_ON_REQ()
            {
                DR_ID = droneId
            };
            // await DroneConnectingStartAsync(msseage, controllerUnit);
        }
    }

    // public async Task DroneConnectingStartAsync(DRONE_NET_ON_REQ droneInfoData, IGcsControllerUnit socketMonitor)
    // {
    //     // DR_ID가 null 이면 연결 시작 종료
    //     if (droneInfoData.DR_ID is null) return;
    //     
    //     // 드론 상태 정보 가져오기
    //     var droneState = GetDroneStateData(droneInfoData.DR_ID, false, socketMonitor);
    //     
    //     // 드론 상태가 존재하는 경우
    //     if (droneState != null)
    //     {
    //         droneState = GetDroneStateData(droneInfoData.DR_ID, true, socketMonitor);
    //         if (droneState is null) return;
    //         await 
    //     }
    // }
}