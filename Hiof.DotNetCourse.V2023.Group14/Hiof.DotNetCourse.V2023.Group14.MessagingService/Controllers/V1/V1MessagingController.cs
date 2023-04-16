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

    [HttpGet("[action]")]
    public async Task<ActionResult> GetByConversationId(Guid id)
    {
        if (id.ToString() != null && !id.Equals(Guid.Empty))
        {
            var conversation = await _messagingService.GetByConversationId(id);
            if (conversation != null)
            {
                return Ok(conversation);
            }
            else
            {
                return NotFound($"Conversation with ID: {id}\ndoes not exists.");
            }
        }
        return BadRequest("Parameter ID cannot be null.");

    }

    [HttpGet("[action]")]
    public async Task<ActionResult<V1ConversationModel>> GetByParticipant(string name)
    {
        try
        {
            var result = await _messagingService.GetByParticipant(name);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"Participant: {name} does not exists.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreateNewConversation(Guid conversationId, [FromBody] IEnumerable<string> participants)
    {
        try
        {
            if (conversationId == Guid.Empty || conversationId.ToString().Length < 36)
            {
                return BadRequest("Conversation id must be at least 36 characters long!");
            }

            if (!participants.Any() || participants.SequenceEqual(new[] { "string" }))
            {
                return BadRequest("Please add at least one participant to the conversation!");
            }

            bool created = await _messagingService.CreateNewConversation(conversationId, participants);

            if (created)
            {
                return Ok("New conversation successfully created!");
            }
            else
            {
                return BadRequest("ConversationId already exists, please provide another one!");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddMessageToConveration(Guid conversationId, string sender, [FromBody] string message)
    {
        if (sender == null || message == null)
        {
            return BadRequest("Sender and message fields cannot be null!");
        }
        else if (conversationId.Equals(Guid.Empty))
        {
            return BadRequest("Please enter a valid conversationId!");
        }
        else if (message.Length > MESSAGE_MAX)
        {
            return BadRequest("Message cannot be longer than 120 characters.");
        }
        else if (message.Equals(String.Empty))
        {
            return BadRequest("Please enter a message to send!");
        }
        else if (sender.Equals(String.Empty))
        {
            return BadRequest("Please enter the name of the sender!");
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
        else if (!Enum.IsDefined(typeof(ReactionType), reaction))
        {
            return BadRequest("Invalid reaction type.");
        }
        else
        {
            await _messagingService.AddReactionToMessage(messageId, reaction);
            return Ok("Reaction added to message successfully!");
        }
    }

    [HttpPut("[action]")]
    public async Task<ActionResult> UpdateMessage(Guid messageId, string newMessage)
    {
        try
        {
            bool detected = await _messagingService.UpdateMessage(messageId, newMessage);
            if (detected)
            {
                return Ok("Message successfully updated!");
            }
            else if (messageId.Equals(Guid.Empty))
            {
                return BadRequest("Message id cannot be empty!"); 
            }
            else if (newMessage.Equals(String.Empty))
            {
                return BadRequest("Please enter the updated message.");
            }
            else
            {
                return BadRequest("Message with the ID does not exists.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteConversation(Guid conversationId)
    {
        try
        {
            bool deleted = await _messagingService.DeleteConversation(conversationId);
            if (deleted)
            {
                return Ok("Conversation successfully deleted!");
            }
            else
            {
                return BadRequest("Conversation with the ID does not exist");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("[action]")]
    public async Task<ActionResult> DeleteMessage(Guid messageId)
    {
        try
        {
            bool detected = await _messagingService.DeleteMessage(messageId);
            if (detected)
            {
                return Ok("Message successfully deleted!");
            }
            else if (messageId.Equals(Guid.Empty))
            {
                return BadRequest("Message id cannot be empty!");
            }
            else
            {
                return BadRequest("Message with the ID does not exists.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
