using Hiof.DotNetCourse.V2023.Group14.MessagingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Controllers;

[ApiController]
[Route("messages/1.0")]
public class V1MessagingController : ControllerBase
{
    private readonly V1MessagingService _messagingService;

    // b9c9cc8a-cc79-4d67-8e24-4b4f4b31d0c1


    public V1MessagingController(V1MessagingService service)
    {
        _messagingService = service;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreateNewConversation(Guid conversationId, [FromBody] IEnumerable<string> participants)
    {
        if (conversationId == Guid.Empty)
        {
            return BadRequest("Conversation id cannot be empty!");
        }

        if (participants == null || !participants.Any())
        {
            return BadRequest("Please add participants to the conversation");
        }

        await _messagingService.CreateNewConversation(conversationId, participants);
        return Ok("New conversation successfully created!");

    }
}

