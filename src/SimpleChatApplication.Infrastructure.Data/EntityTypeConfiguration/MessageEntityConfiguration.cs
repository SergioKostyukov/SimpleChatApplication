using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleChatApplication.Infrastructure.Data.EntityTypeConfiguration;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ChatId)
            .IsRequired();

        builder.Property(x => x.SenderId)
            .IsRequired();

        builder.Property(x => x.Body)
            .IsRequired();

        builder.Property(x => x.SentTime)
            .IsRequired();
    }
}
