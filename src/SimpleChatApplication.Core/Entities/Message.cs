namespace SimpleChatApplication.Core.Entities;

public class Message
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime SentTime { get; set; }
}
