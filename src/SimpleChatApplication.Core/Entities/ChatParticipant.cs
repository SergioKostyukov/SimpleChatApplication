using SimpleChatApplication.Core.Enums;

namespace SimpleChatApplication.Core.Entities;

public class ChatParticipant
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public DateTime JoinedTime { get; set; }
    public ChatUserRole Role { get; set; }
}
