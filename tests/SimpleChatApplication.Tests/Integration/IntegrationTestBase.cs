using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.Core.Entities;
using SimpleChatApplication.Infrastructure.Data;
using Xunit;

namespace SimpleChatApplication.Tests.Integration;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;
    protected IServiceProvider ServiceProvider;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        var inMemoryFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ChatDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ChatDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ChatDbContext>();

                    db.Database.EnsureCreated();
                }

                ServiceProvider = sp;
            });
        });

        _client = inMemoryFactory.CreateClient();
    }

    protected async Task SeedDatabase()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();

            context.Chats.Add(new Chat
            {
                Title = "Test Chat",
                CreatorId = 1,
                CreationTime = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var chatCount = context.Chats.Count();
            Assert.True(chatCount > 0, "SeedDatabase: Chats were not added to the database.");
        }
    }
}
