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
    }
}
