using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleChatApplication.Infrastructure.Data.EntityTypeConfiguration;

internal class ChatEntityConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatorId)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired();

        builder.Property(x => x.CreationTime)
            .IsRequired();

        // Налаштовуємо зв'язок один-до-багатьох між Chat та Message
        builder.HasMany(c => c.Messages)
               .WithOne(m => m.Chat)
               .HasForeignKey(m => m.ChatId)
               .OnDelete(DeleteBehavior.Cascade);

        // Налаштовуємо зв'язок один-до-багатьох між Chat та ChatParticipant
        builder.HasMany(c => c.Participants)
               .WithOne(p => p.Chat)
               .HasForeignKey(p => p.ChatId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
