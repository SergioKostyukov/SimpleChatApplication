namespace SimpleChatApplication.Application.Dto;

public class ChatViewDto
{
    public int Id { get; init; }
    public int CreatorId { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime CreationTime { get; init; }
    public bool ActiveAdminStatus { get; set; } = false;
}
