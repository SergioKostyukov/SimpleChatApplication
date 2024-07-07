namespace SimpleChatApplication.Application.Dto;

public record ChatDto(
    int Id,
    int CreatorId,
    string Title,
    DateTime CreationTime,
    List<MessageDto> Messages,
    List<ParticipantDto> Participants
);
