using MessageService.BLL.Interfaces;
using MessageService.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace MessageService.BLL.Services;

public class WebSocketHandler : IWebSocketHandler
{
    private readonly List<WebSocket> _sockets = new();
    private readonly ILogger<WebSocketHandler> _logger;

    public WebSocketHandler(ILogger<WebSocketHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket, CancellationToken token)
    {
        _sockets.Add(webSocket);
        _logger.LogInformation("A new client has connected via WebSocket.");
        
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                _logger.LogInformation("The client has disconnected from the WebSocket.");
                
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing the connection",
                    CancellationToken.None);
                _sockets.Remove(webSocket);
            }
        }
    }

    public async Task BroadcastMessageAsync(Message message, CancellationToken token)
    {
        var json = JsonSerializer.Serialize(message);
        
        var data = Encoding.UTF8.GetBytes(json);
        var tasks = _sockets
            .Where(ws => ws.State == WebSocketState.Open)
            .Select(ws =>
                ws.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, token));
        await Task.WhenAll(tasks);
    }
}