using System.ComponentModel.DataAnnotations;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Controllers;

[ApiController]
[Route("messages/1.0")]
public class V1MessagingController : ControllerBase
{
    private readonly V1MessagingService _messagingService;
    private const int MESSAGE_MAX = 120;

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

    [HttpPost("[action]")]
    public async Task<ActionResult> AddMessageToConveration([Required] Guid conversationId, string sender, string message)
    {
        if (sender == null || message == null)
        {
            return BadRequest("Sender and message fields cannot be empty!");
        }
        else if (message.Length > MESSAGE_MAX)
        {
            return BadRequest("Message cannot be longer than 120 characters.");
        }
        else
        {
            await _messagingService.AddMessageToConversation(conversationId, sender, message);
            return Ok("Message added successfully to the conversation!");

        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddReactionToMessage(Guid messageId, ReactionType reaction)
    {
        if (messageId == Guid.Empty)
        {
            return BadRequest("MessageId cannot be empty.");
        }
        else if (messageId.ToString().Length > 36)
        {
            return BadRequest("MessageId cannot be longer than 36 characters");
        }
        else
        {
            await _messagingService.AddReactionToMessage(messageId, reaction);
            return Ok("Reaction added to message successfully!");
        }
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult> DeleteConversation(Guid conversationId)
    {
        if (conversationId.ToString() != null)
        {
            await _messagingService.DeleteConversation(conversationId);
            return Ok("Conversation successfully deleted!");
        }
        else
        {
            return BadRequest("ConversationId cannot be null");
        }
        
    }
}
