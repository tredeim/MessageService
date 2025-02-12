using MessageService.Models;
using Npgsql;

namespace MessageService.DAL;

public class MessageRepository : IMessageRepository
{
    private readonly string _connectionString;

    public MessageRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertMessageAsync(Message message, CancellationToken token)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var command = new NpgsqlCommand(
            "INSERT INTO messages (text, created_at, sequence_number) " +
            "           VALUES (@text, @created_at, @seq) " +
            "           RETURNING id",
            conn);

        command.Parameters.AddWithValue("text", message.Text);
        command.Parameters.AddWithValue("created_at", message.CreatedAt);
        command.Parameters.AddWithValue("seq", message.SequenceNumber);

        var result = await command.ExecuteScalarAsync(token);
        
        if (result != null && result != DBNull.Value)
        {
            message.Id = Convert.ToInt32(result);
        }
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to, CancellationToken token)
    {
        var messages = new List<Message>();
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var command = new NpgsqlCommand(
            "SELECT id, text, created_at, sequence_number FROM messages " +
            "       WHERE created_at BETWEEN @from AND @to " +
            "       ORDER BY created_at ASC",
            conn);

        command.Parameters.AddWithValue("from", from);
        command.Parameters.AddWithValue("to", to);
        using var reader = await command.ExecuteReaderAsync(token);
        while (await reader.ReadAsync())
        {
            messages.Add(new Message
            {
                Id = reader.GetInt32(0),
                Text = reader.GetString(1),
                CreatedAt = reader.GetDateTime(2),
                SequenceNumber = reader.GetInt32(3)
            });
        }

        return messages;
    }
}
