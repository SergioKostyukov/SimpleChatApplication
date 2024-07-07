using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.Application.MappingProfiles;

namespace SimpleChatApplication.Application;

public static class RegistrationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}
