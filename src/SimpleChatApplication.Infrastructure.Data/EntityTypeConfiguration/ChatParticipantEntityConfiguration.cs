using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleChatApplication.Infrastructure.Data.EntityTypeConfiguration;

internal class ChatParticipantEntityConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.HasKey(x => new { x.ChatId, x.UserId });

        builder.Property(x => x.JoinedTime)
            .IsRequired();

        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<int>();
    }
}
