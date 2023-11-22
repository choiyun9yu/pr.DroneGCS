using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

public class SignalRHub : Hub
{
    // 클라이언트로부터 호출되는 메서드
    public async Task SendMessage(string user, string message)
    {
        // 모든 클라이언트에게 'ReceiveMessage' 메서드를 호출하여 데이터 전송
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}