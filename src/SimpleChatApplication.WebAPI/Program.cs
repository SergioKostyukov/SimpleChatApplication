using SimpleChatApplication.Infrastructure.Data;
using SimpleChatApplication.Application;
using SignalRChat.Hubs;

namespace SimpleChatApplication;

public class Program
{
    public static void Main(string[] args)
    {
        #region Configure services
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddStorage(builder.Configuration);

        builder.Services.AddServices();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSignalR();

        #endregion

        #region Configure pipeline
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();
        app.MapHub<ChatHub>("/chatHub");

        app.Run();
        #endregion
    }
}