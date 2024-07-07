namespace SimpleChatApplication.Application.Dto;

public record ChatCreateDto(
    int CreatorId,
    string Title,
    DateTime CreationTime
);
