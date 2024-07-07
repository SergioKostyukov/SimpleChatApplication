using Microsoft.AspNetCore.Mvc;

namespace SimpleChatApplication.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChatController(ILogger<ChatController> logger) : ControllerBase
    {
        private readonly ILogger<ChatController> _logger = logger;
    }
}
