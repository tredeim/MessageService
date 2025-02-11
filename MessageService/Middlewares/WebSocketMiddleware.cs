using MessageService.BLL.Interfaces;

namespace MessageService.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebSocketHandler _webSocketHandler;
    
    public WebSocketMiddleware(RequestDelegate next, IWebSocketHandler webSocketHandler)
    {
        _next = next;
        _webSocketHandler = webSocketHandler;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws/messages")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await _webSocketHandler.HandleWebSocketAsync(context, socket, context.RequestAborted);
            }
            else
                context.Response.StatusCode = 400;
        }
        else
            await _next(context);
    }
}
