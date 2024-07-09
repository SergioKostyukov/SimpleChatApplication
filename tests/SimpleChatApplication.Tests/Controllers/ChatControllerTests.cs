using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleChatApplication.Application;
using SimpleChatApplication.Application.Dto;
using SimpleChatApplication.WebAPI.Controllers;
using Xunit;

namespace SimpleChatApplication.Tests.Controllers;

public class ChatServiceTests
{
    private readonly Mock<IChatService> _chatServiceMock = new();
    private ChatController _controller;

    #region Get
    [Fact]
    public async Task Get_ReturnsOkResult_ValidChatId()
    {
        // Arrange
        var expectedChatInfo = new ChatDto(
                           Id: 1,
                           CreatorId: 1,
                           Title: "Test Chat",
                           CreationTime: DateTime.UtcNow,
                           Messages: [],
                           Participants: []);

        _chatServiceMock.Setup(s => s.GetById(It.IsAny<int>()))
                       .ReturnsAsync(expectedChatInfo);

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var anonymousValue = okResult.Value;
        Assert.NotNull(anonymousValue);

        var chatProperty = anonymousValue.GetType().GetProperty("Chat");
        Assert.NotNull(chatProperty);

        var chat = chatProperty.GetValue(anonymousValue) as ChatDto;
        Assert.NotNull(chat);
        Assert.Equal(expectedChatInfo, chat);
    }

    [Fact]
    public async Task Get_ReturnsNotFoundResult_InvalidChatId()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.GetById(It.IsAny<int>()))
                       .ThrowsAsync(new KeyNotFoundException("Chat not found"));

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Chat not found", notFoundResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsBadRequestResult_ExceptionThrown()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.GetById(It.IsAny<int>()))
                       .ThrowsAsync(new Exception("Unexpected error"));

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Unexpected error", badRequestResult.Value);
    }
    #endregion

    #region Create
    [Fact]
    public async Task Create_ReturnsOkResult_NoServerErrors()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.Create(It.IsAny<ChatCreateDto>()))
                       .ReturnsAsync(1);

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Create(1, "New Chat");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Chat with ID 1 created", okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequestResult_ExceptionThrown()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.Create(It.IsAny<ChatCreateDto>()))
                       .ThrowsAsync(new Exception("Unexpected error"));

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Create(1, "New Chat");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Unexpected error", badRequestResult.Value);
    }
    #endregion

    #region ConnectTo
    [Fact]
    public async Task ConnectTo_ReturnsOkResult_ValidParameters()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.Connect(It.IsAny<int>(), It.IsAny<int>()))
                       .Returns(Task.CompletedTask); // Since it returns void

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.ConnectTo(1, 1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ConnectTo_ReturnsBadRequestResult_ExceptionThrown()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.Connect(It.IsAny<int>(), It.IsAny<int>()))
                       .ThrowsAsync(new Exception("Unexpected error"));

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.ConnectTo(1, 1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Unexpected error", badRequestResult.Value);
    }
    #endregion

    #region Delete
    [Fact]
    public async Task Delete_ReturnsOkResult_ValidParameters()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.Delete(It.IsAny<int>(), It.IsAny<int>()))
                       .Returns(Task.CompletedTask); // Since it returns void

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Delete(1, 1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequestResult_ExceptionThrown()
    {
        // Arrange
        _chatServiceMock.Setup(s => s.Delete(It.IsAny<int>(), It.IsAny<int>()))
                       .ThrowsAsync(new Exception("Unexpected error"));

        _controller = new ChatController(_chatServiceMock.Object);

        // Act
        var result = await _controller.Delete(1, 1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Unexpected error", badRequestResult.Value);
    }
    #endregion
}