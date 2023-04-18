using System;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Services
{
	public class V1MessagingService : V1IMessages
	{
        private readonly MessagingContext _context;

        private readonly ILogger<V1MessagingService> _logger;

		public V1MessagingService(MessagingContext context, ILogger<V1MessagingService> logger)
		{
            _context = context;
            _logger = logger;
		}

        public async Task AddMessageToConversation(Guid conversationId, string sender, string message)
        {
            try
            {
                var conversation = await _context.ConversationModel
                    .Include(x => x.Messages)
                    .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

                if (conversation == null)
                {
                    // Conversation not found, create new conversation
                    conversation = new V1ConversationModel
                    {
                        ConversationId = conversationId,
                        Participants = new List<V1Participant>(),
                        Messages = new List<V1Messages>()
                    };

                    await _context.ConversationModel.AddAsync(conversation);
                }

                var messageToAdd = new V1Messages
                {
                    MessageId = Guid.NewGuid(),
                    Sender = sender,
                    Message = message,
                    Date = DateTime.UtcNow,
                    Reactions = new List<V1Reactions>()
                };

                // Add the message to the conversation
                conversation.Messages.Add(messageToAdd);
                await _context.Messages.AddAsync(messageToAdd);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add message to database.");
            }
        }

        public async Task AddReactionToMessage(Guid messageId, ReactionType reaction)
        {
            try
            {
                var message = await _context.Messages
                    .Include(x => x.Reactions)
                    .FirstOrDefaultAsync(x => x.MessageId == messageId);

                if (message == null)
                {
                    throw new ArgumentException("Message not found", nameof(messageId));
                }

                var messageReaction = new V1Reactions
                {
                    ReactionId = Guid.NewGuid(),
                    MessageId = messageId,
                    Type = reaction
                };

                message.Reactions.Add(messageReaction);
                await _context.MessageReaction.AddAsync(messageReaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add reaction to message.");
            }
            
        }

        public async Task<bool> CreateNewConversation(Guid conversationId, IEnumerable<string> participants)
        {
            try
            {
                bool existingConversation = await _context.ConversationModel.AnyAsync(x => x.ConversationId == conversationId);

                if (existingConversation)
                {
                    throw new Exception("ConversationId already exists. Please provide another one!");
                }

                // Create a new conversation
                var conversation = new V1ConversationModel
                {
                    ConversationId = conversationId,
                    Participants = new List<V1Participant>(),
                    Messages = new List<V1Messages>()
                };

                // Add participants to the participants table.
                foreach (var participant in participants)
                {
                    var newParticipant = new V1Participant
                    {
                        Participant = participant,
                        ConversationId = conversationId
                    };

                    conversation.Participants.Add(newParticipant);
                }

                await _context.ConversationModel.AddAsync(conversation);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add conversation to database.");
                return false;
            }
        }

        public async Task<bool> DeleteConversation(Guid conversationId)
        {
            try
            {
                var conversation = await _context.ConversationModel.FindAsync(conversationId);
                if (conversation != null)
                {
                    _context.Remove(conversation);
                    var rowsAffected = await _context.SaveChangesAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Failed to delete conversation.");
                    }
                    return true;
                }
                else
                {
                    throw new ArgumentException("Conversation with the ID does not exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete the conversation.");
                return false;
            }
        }

        public async Task<bool> DeleteMessage(Guid messageId)
        {
            try
            {
                var message = await _context.Messages.FindAsync(messageId);
                if (message != null)
                {
                    _context.Remove(message);
                    var rowsAffected = await _context.SaveChangesAsync();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Failed to delete message!");
                    }
                    return true;
                }
                else
                {
                    throw new ArgumentException("Message with the ID does not exists.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not delete the message.");
                return false;
            }
            
        }

        public async Task<V1ConversationModel?> GetByConversationId(Guid conversationId)
        {
            try
            {
                return await _context.ConversationModel
                    .Include(x => x.Participants)
                    .Include(x => x.Messages.OrderBy(m => m.Date))
                    .FirstOrDefaultAsync(x => x.ConversationId == conversationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get the conversation from the database.");
                return null;
            }
           
        }

        public async Task<List<V1ConversationModel>?> GetByParticipant(string participant)
        {
            try
            {
                var conversation = await _context.ConversationModel
                    .Where(c => c.Participants.Any(p => p.Participant == participant))
                    .Include(c => c.Participants)
                    .Include(c => c.Messages.OrderBy(m => m.Date))
                    .ToListAsync();

                if (conversation == null)
                {
                    throw new Exception($"Conversation with participant '{participant}' not found.");
                }

                return conversation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not return conversation for participant: {participant}");
                return null;
            }
        }

        public async Task<bool> UpdateMessage(Guid messageId, string message)
        {
            try
            {
                var existing = await _context.Messages
               .Where(m => m.MessageId == messageId)
               .FirstOrDefaultAsync();

                if (existing == null)
                {
                    throw new ArgumentException("Message with the ID does not exist");
                }

                existing.Message = message;
                var rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("Failed to update message.");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update the message.");
                return false;
            }
        }

        public async Task<bool> UpdateIsRead(Guid conversationId, string participantId, bool isRead)
        {
            try
            {
                var existing = await _context.Participant.Where(c => c.ConversationId == conversationId).Where(p => p.Participant == participantId).FirstOrDefaultAsync();                    

                if (existing == null)
                {
                    throw new ArgumentException("Conversation with the ID does not exist");
                }

                existing.IsRead = isRead;
                var rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected == 0)
                {
                    throw new Exception("Failed to update message.");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update the message.");
                return false;
            }
        }

        Task<V1ConversationModel?> V1IMessages.GetByParticipant(string participant)
        {
            throw new NotImplementedException();
        }
    }
}

