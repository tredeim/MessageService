using MessageService.BLL.Interfaces;
using MessageService.DAL;
using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageRepository _repository;
    private readonly ILogger<MessageController> _logger;
    private readonly IWebSocketHandler _webSocketHandler;
    
    public MessageController(IMessageRepository repository, ILogger<MessageController> logger, IWebSocketHandler webSocketHandler)
    {
        _repository = repository;
        _logger = logger;
        _webSocketHandler = webSocketHandler;
    }

    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request, CancellationToken token)
    {
        if (string.IsNullOrEmpty(request.Text) || request.Text.Length > 128)
        {
            _logger.LogWarning("Invalid message: empty or exceeds allowable length.");
            
            return BadRequest("Invalid message");
        }
        
        var message = new Message
        {
            Text = request.Text,
            SequenceNumber = request.SequenceNumber,
            CreatedAt = DateTime.UtcNow
        };
        
        await _repository.InsertMessageAsync(message, token);
        _logger.LogInformation("The message is saved in the database.");
        
        await _webSocketHandler.BroadcastMessageAsync(message, token);
        
        return Ok(message);
    }
    
    [HttpGet]
    [Route("history")]
    public async Task<IActionResult> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to,  CancellationToken token)
    {
        var messages = await _repository.GetMessagesAsync(from, to, token);
        
        return Ok(messages);
    }
}
