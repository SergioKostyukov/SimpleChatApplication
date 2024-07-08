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

    public async Task SendMessage(int chatId, int userId, string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
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
            _logger.LogInformation("Message sent to group");
        }
        else
        {
            _logger.LogWarning("Empty message received.");
        }
    }


    public async Task JoinChat(int chatId, int userId)
    {
        if (await IsUserInChat(chatId, userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            _logger.LogInformation("Joined chat group");
        }
    }

    public async Task LeaveChat(int chatId, int userId)
    {
        if (await IsUserInChat(chatId, userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
            _logger.LogInformation("Left chat group");
        }
    }

    private async Task<bool> IsUserInChat(int chatId, int userId)
    {
        var isUserInChat = await _dbContext.ChatParticipants
                            .AnyAsync(x => x.ChatId == chatId && x.UserId == userId);

        if (!isUserInChat)
            _logger.LogWarning("User not part of the chat");

        return isUserInChat;
    }
}