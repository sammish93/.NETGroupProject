using Hiof.DotNetCourse.V2023.Group14.MessagingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Controllers;

[ApiController]
[Route("messages/1.0")]
public class V1MessagingController : ControllerBase
{
    private readonly V1MessagingService _messagingService;

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
        else if (conversationId.ToString().Length < 36)
        {
            return BadRequest("Id must be at least 36 characters long.");
        }

        if (participants == null || !participants.Any())
        {
            return BadRequest("Please add participants to the conversation");
        }

        await _messagingService.CreateNewConversation(conversationId, participants);
        return Ok("New conversation successfully created!");

    }

    // TODO: Implement the rest of the methods from the V1MessagingService.
}

