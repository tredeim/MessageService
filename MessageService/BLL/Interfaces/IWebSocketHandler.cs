using System.Net.WebSockets;
using MessageService.Models;

namespace MessageService.BLL.Interfaces;

public interface IWebSocketHandler
{
    Task BroadcastMessageAsync(Message message, CancellationToken token);
    Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket, CancellationToken token);
}