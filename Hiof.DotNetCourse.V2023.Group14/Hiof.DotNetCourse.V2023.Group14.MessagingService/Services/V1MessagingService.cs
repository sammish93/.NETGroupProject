﻿using System;
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

        public async Task AddReactionToMessage(Guid messageId, V1Reactions reaction)
        {
            // TODO: Check and handle exceptions.

            var message = await _context.Messages.FindAsync(messageId);

            if (message == null)
            {
                throw new ArgumentException("Message not found", nameof(messageId));
            }

            var reactionId = Guid.NewGuid();
            var messageReaction = new V1Reactions
            {
                ReactionId = reactionId,
                Type = reaction.Type,
                MessageId = messageId
            };

            _context.MessageReaction.Add(messageReaction);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewConversation(Guid conversationId, IEnumerable<string> participants)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add conversation to database.");
            }
        }

        public Task DeleteConversation(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessage(Guid conversationId, Guid messageId)
        {
            throw new NotImplementedException();
        }

        public async Task<V1ConversationModel?> GetByConversationId(Guid participantId)
        {
            return await _context.ConversationModel.FindAsync(participantId);
        }

        public Task<V1ConversationModel?> GetByParticipant(string participant)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExistingMessage(Guid conversationId, Guid messageId, string message)
        {
            throw new NotImplementedException();
        }
    }
}

