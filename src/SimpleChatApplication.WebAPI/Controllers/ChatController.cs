using Microsoft.AspNetCore.Mvc;
using SimpleChatApplication.Application;
using SimpleChatApplication.Application.Dto;
using System.Security.Authentication;

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

            return Ok(new { ChatsList = Chats });
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int chatId)
        {
            try
            {
                ChatDto Chat = await _chatService.GetById(chatId);

                return Ok(new { ChatDetails = Chat });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userId, string chatTitle)
        {
            try
            {
                await _chatService.Create(new ChatCreateDto(
                    CreatorId: userId,
                    Title: chatTitle,
                    CreationTime: DateTime.UtcNow
                ));

                return Ok();
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
            catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidDataException)
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
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidDataException)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
