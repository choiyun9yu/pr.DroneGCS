using kisa_gcs_system.Models;

namespace SignalR.Hubs
{
    public class DroneHub : Hub
    {
        public async Task DroneStateUpdate(object message)
        {
            // 클라이언트로부터 메시지를 받으면 이 메소드를 호출하여 메시지를 모든 연결된 클라이언트에게 전송합니다.
            await Clients.All.SendAsync("DroneStateUpdate", message);
        }
    }
}