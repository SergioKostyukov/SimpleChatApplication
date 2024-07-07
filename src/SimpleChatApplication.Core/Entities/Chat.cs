namespace SimpleChatApplication.Core.Entities;

public class Chat
{
    public int Id { get; set; }
    public int CreatorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }

    public ICollection<Message> Messages { get; set; }
    public ICollection<ChatParticipant> Participants { get; set; }
}
