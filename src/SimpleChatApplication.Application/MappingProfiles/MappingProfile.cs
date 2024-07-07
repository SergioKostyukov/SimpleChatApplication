using AutoMapper;
using SimpleChatApplication.Application.Dto;
using SimpleChatApplication.Core.Entities;

namespace SimpleChatApplication.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ChatCreateDto, Chat>();
        CreateMap<Chat, ChatDto>();
        CreateMap<Message, MessageDto>();
        CreateMap<ChatParticipant, ParticipantDto>();
    }
}
