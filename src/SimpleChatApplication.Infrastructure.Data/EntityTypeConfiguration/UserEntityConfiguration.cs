using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleChatApplication.Infrastructure.Data.EntityTypeConfiguration;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
