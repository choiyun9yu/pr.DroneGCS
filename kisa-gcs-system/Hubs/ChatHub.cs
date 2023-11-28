using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)  // 클라이언트로부터 호출되는 메서드
        {
            Console.WriteLine($"{user} sent {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);   // 모든 클라이언트에게 'ReceiveMessage' 메서드를 호출하여 데이터 전송
        }
    }

    public class DroneHub : Hub
    {
        public async Task SendMessageToClient(string message)
        {
            // 클라이언트에게 메시지 보내기
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}