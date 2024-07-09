using Microsoft.AspNetCore.Mvc;
using SimpleChatApplication.Application;
using SimpleChatApplication.Application.Dto;

namespace SimpleChatApplication.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChatController(IChatService chatService) : ControllerBase
    {
        private readonly IChatService _chatService = chatService;

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int userId)
        {
            List<ChatViewDto> Chats = await _chatService.GetList(userId);

            return Ok(Chats);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int chatId)
        {
            try
            {
                ChatDto Chat = await _chatService.GetById(chatId);

                return Ok(new { Chat });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, string chatTitle)
        {
            try
            {
                int chatId = await _chatService.Create(new ChatCreateDto(
                    CreatorId: userId,
                    Title: chatTitle,
                    CreationTime: DateTime.UtcNow
                ));

                return Ok($"Chat with ID {chatId} created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConnectTo(int chatId, int userId)
        {
            try
            {
                await _chatService.Connect(chatId, userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int chatId, int userId)
        {
            try
            {
                await _chatService.Delete(chatId, userId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
