namespace MessageService.Models;

public class SendMessageRequest
{
    public string Text { get; set; }
    public int SequenceNumber { get; set; }
}