using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleChatApplication.Application.Dto;
using SimpleChatApplication.Core.Entities;
using SimpleChatApplication.Infrastructure.Data;
using System.Security.Authentication;

namespace SimpleChatApplication.Application;

internal class ChatService(ILogger<ChatService> logger,
                           ChatDbContext dbContext,
                           IMapper mapper) : IChatService
{
    private readonly ILogger<ChatService> _logger = logger;
    private readonly ChatDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<List<ChatViewDto>> GetList(int userId)
    {
        _logger.LogInformation($"Getting chat list for user ID: {userId}");

        var chats = await _dbContext.Chats
        .Select(c => new ChatViewDto
        {
            Id = c.Id,
            Title = c.Title,
            CreatorId = c.CreatorId,
            CreationTime = c.CreationTime,
            ActiveAdminStatus = c.CreatorId == userId
        })
        .ToListAsync();

        return chats;
    }

    public async Task<ChatDto> GetById(int chatId)
    {
        _logger.LogInformation($"Getting chat by ID: {chatId}");

        var chat = await _dbContext.Chats
            .Include(c => c.Messages)
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == chatId) ?? throw new KeyNotFoundException($"Chat with ID {chatId} not found.");

        return _mapper.Map<ChatDto>(chat);
    }

    public async Task<int> Create(ChatCreateDto request)
    {
        _logger.LogInformation($"Creating new chat with title: {request.Title}");

        var newChat = _mapper.Map<Chat>(request);

        await _dbContext.Chats.AddAsync(newChat);
        await _dbContext.SaveChangesAsync();

        var chatId = newChat.Id;
        if (chatId == 0)
        {
            throw new InvalidOperationException("Failed to retrieve the generated ChatId.");
        }

        var chatParticipant = new ChatParticipant
        {
            ChatId = chatId,
            UserId = request.CreatorId,
            JoinedTime = DateTime.UtcNow,
            Role = Core.Enums.ChatUserRole.Admin
        };

        await _dbContext.ChatParticipants.AddAsync(chatParticipant);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Chat with ID {chatId} created");

        return chatId;
    }

    public async Task Connect(int chatId, int userId)
    {
        _logger.LogInformation($"Connecting user {userId} to chat {chatId}");

        var chat = await _dbContext.Chats
            .FindAsync(chatId) ?? throw new KeyNotFoundException($"Chat with ID {chatId} not found.");

        var isUserInChat = await _dbContext.ChatParticipants
                        .AnyAsync(x => x.ChatId == chatId && x.UserId == userId);

        if (isUserInChat)
            throw new InvalidDataException("User already participated");

        var chatParticipant = new ChatParticipant
        {
            ChatId = chatId,
            UserId = userId,
            JoinedTime = DateTime.UtcNow,
            Role = Core.Enums.ChatUserRole.Member
        };

        await _dbContext.ChatParticipants.AddAsync(chatParticipant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int chatId, int userId)
    {
        _logger.LogInformation($"Deleting chat {chatId} by user {userId}");

        var chat = await _dbContext.Chats
            .FindAsync(chatId) ?? throw new KeyNotFoundException($"Chat with ID {chatId} not found.");

        var admin = await _dbContext.ChatParticipants
                            .FirstOrDefaultAsync(x => x.ChatId == chatId &&
                                                  x.Role == Core.Enums.ChatUserRole.Admin)
                            ?? throw new InvalidDataException("No admin in the chat");

        if (admin.UserId != userId) throw new InvalidCredentialException("User is not an admin");

        _dbContext.Chats.Remove(chat);
        await _dbContext.SaveChangesAsync();
    }
}
