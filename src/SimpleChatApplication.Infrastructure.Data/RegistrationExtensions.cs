using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleChatApplication.Infrastructure.Data;

public static class RegistrationExtensions
{
    public static void AddStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ChatDbContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnectionString"],
                options => options.MigrationsAssembly(typeof(ChatDbContext).Assembly.FullName));
        });

        // place for data generation extension
    }
}
