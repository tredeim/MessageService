namespace MessageService.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SequenceNumber { get; set; }
}
