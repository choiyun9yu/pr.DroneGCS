using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class DroneHub : Hub
    {
        // 클라이언트로 메시지를 보내는 예제 이벤트 핸들러
        public async Task SendEventToClient(string? mavMessage)
        {
            // 클라이언트로 메시지를 보냅니다.
            await Clients.All.SendAsync("ReceiveMavMessage", mavMessage);
        }
    }
    
    
    
    
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)  // 클라이언트로부터 호출되는 메서드
        {
            Console.WriteLine($"{user} sent {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);   // 모든 클라이언트에게 'ReceiveMessage' 메서드를 호출하여 데이터 전송
        }
    }


}