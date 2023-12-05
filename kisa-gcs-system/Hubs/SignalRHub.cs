using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class DroneHub : Hub
    {
        public async Task SendMavMessage(string message)
        {
            // 클라이언트로부터 메시지를 받으면 이 메소드를 호출하여 메시지를 모든 연결된 클라이언트에게 전송합니다.
            await Clients.All.SendAsync("ReceiveMavMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            // 클라이언트가 연결될 때 호출됩니다.
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // 클라이언트가 연결 해제될 때 호출됩니다.
            await base.OnDisconnectedAsync(exception);
        }

        // // 클라이언트로 메시지를 보내는 예제 이벤트 핸들러
    // public async Task SendEventToClient(string? mavMessage)
    // {
    //     // 클라이언트로 메시지를 보냅니다.
    //     await Clients.All.SendAsync("ReceiveMavMessage", mavMessage);
    // }
    
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