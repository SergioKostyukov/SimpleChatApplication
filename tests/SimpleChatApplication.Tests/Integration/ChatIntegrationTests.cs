using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SimpleChatApplication.Application.Dto;
using SimpleChatApplication.Infrastructure.Data;
using SimpleChatApplication.WebAPI.Controllers;
using System.Reflection;
using System.Text;
using Xunit;

namespace SimpleChatApplication.Tests.Integration;

public class ChatControllerTests : IntegrationTestBase
{
    public ChatControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task GetList_ReturnsChatList()
    {
        // Arrange
        await SeedDatabase();

        // Act
        var response = await _client.GetAsync("/api/Chat/GetList?userId=1");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ChatViewDto>>(content);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}
