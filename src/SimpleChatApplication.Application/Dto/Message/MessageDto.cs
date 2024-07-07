namespace SimpleChatApplication.Application.Dto;

public record MessageDto(
    int Id,
    int ChatId,
    int SenderId,
    string Body,
    DateTime SentTime
);
