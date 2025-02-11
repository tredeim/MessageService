using MessageService.Models;

namespace MessageService.DAL;

public interface IMessageRepository
{
    Task InsertMessageAsync(Message message, CancellationToken token);
    Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to, CancellationToken token);
}