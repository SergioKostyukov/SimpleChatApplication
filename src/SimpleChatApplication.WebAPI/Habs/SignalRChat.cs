using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.Core.Entities;
using SimpleChatApplication.Infrastructure.Data;

namespace SignalRChat.Hubs;

public sealed class ChatHub(ILogger<ChatHub> logger,
                            ChatDbContext dbContext) : Hub
{
    private readonly ILogger<ChatHub> _logger = logger;
    private readonly ChatDbContext _dbContext = dbContext;

    /// <summary>
    /// Sends a message to a chat group.
    /// </summary>
    /// <param name="chatId">The ID of the chat.</param>
    /// <param name="userId">The ID of the user sending the message.</param>
    /// <param name="message">The message content.</param>
    public async Task SendMessage(int chatId, int userId, string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            _logger.LogWarning("Empty message received.");
            return;
        }

        var chat = await _dbContext.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat == null)
        {
            _logger.LogWarning($"Chat with ID {chatId} not found.");
            return;
        }

        var newMessage = new Message
        {
            ChatId = chat.Id,
            SenderId = userId,
            Body = message,
            SentTime = DateTime.UtcNow
        };

        _dbContext.Messages.Add(newMessage);
        await _dbContext.SaveChangesAsync();

        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", userId, message, newMessage.SentTime);
        _logger.LogInformation($"Message sent to group {chatId}");
    }


    /// <summary>
    /// Allows a user to join a chat group.
    /// </summary>
    /// <param name="chatId">The ID of the chat.</param>
    /// <param name="userId">The ID of the user joining the chat.</param>
    public async Task JoinChat(int chatId, int userId)
    {
        if (await IsUserInChat(chatId, userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());

            _logger.LogInformation($"User {userId} joined to chat {chatId}");
        }
    }

    /// <summary>
    /// Allows a user to leave a chat group.
    /// </summary>
    /// <param name="chatId">The ID of the chat.</param>
    /// <param name="userId">The ID of the user leaving the chat.</param>
    public async Task LeaveChat(int chatId, int userId)
    {
        if (await IsUserInChat(chatId, userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());

            _logger.LogInformation($"User {userId} left chat {chatId}");
        }
    }

    /// <summary>
    /// Checks if a user is part of a chat.
    /// </summary>
    /// <param name="chatId">The ID of the chat.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>True if the user is in the chat, false otherwise.</returns>
    private async Task<bool> IsUserInChat(int chatId, int userId)
    {
        var isUserInChat = await _dbContext.ChatParticipants
                            .AnyAsync(x => x.ChatId == chatId && x.UserId == userId);

        if (!isUserInChat)
            _logger.LogWarning($"User {userId} not part of the chat {chatId}");

        return isUserInChat;
    }
}