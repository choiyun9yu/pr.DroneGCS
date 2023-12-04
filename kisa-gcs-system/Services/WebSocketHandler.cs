using System.Net.WebSockets;
using System.Text;

namespace kisa_gcs_service.Service;

public class WebSocketHandler
{
    private readonly RequestDelegate _next;

    public WebSocketHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketAsync(context, webSocket);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket)
    {
        // 웹 소켓 처리 로직을 구현
        // 예: 메시지 수신 및 전송
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result;

        do
        {
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {message}");

                // 메시지 전송
                var responseMessage = $"Server received: {message}";
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        } while (!result.CloseStatus.HasValue);

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}