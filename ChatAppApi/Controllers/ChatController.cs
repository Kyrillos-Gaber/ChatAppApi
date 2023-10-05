using chatapi.Dtos;
using chatapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chatapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("register")]
    public IActionResult RegisterUser(UserDto user)
    {
        if (ModelState.IsValid)
            if(_chatService.AddUserToList(user.Name))
                return NoContent();
        return BadRequest("model not valid or user is exists");
    }
}
