using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.Core.Entities;

namespace SimpleChatApplication.Infrastructure.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
    public ChatDbContext() { }
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var assembly = typeof(ChatDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }

}
