using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.DTO.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
  	public class MessageChecker
	{
		private readonly ILogger<MessageChecker> _logger;
        private readonly MessagingContext _context;
        private readonly IHubContext<MessageHub> _hubContext;

		public MessageChecker(ILogger<MessageChecker> logger, MessagingContext context, IHubContext<MessageHub> hubContext)
		{
			_logger = logger;
            _context = context;
            _hubContext = hubContext;
		}

        public void CheckMessages(string currentUserId)
		{
            _logger.LogInformation("Background job for checking messages every 5 secounds activated.");

            // By adding the 'currentUserId' we can control that only one background job is
            // running for a specific user at time.
			RecurringJob.AddOrUpdate(currentUserId, () => MessageJob(currentUserId), cronExpression: "*/5 * * * * *");
		}

        // The parameter should represent the currently logged in user.
        public async Task<List<V1Participant>> GetNewMessages(string currentUserId)
        {
            // Get conversation where current user is a participant.
            var userConversation = await _context.Participant
                .Where(p => p.Participant == currentUserId && !p.IsRead
                    && _context.Messages.Any(m => m.ConversationId == p.ConversationId
                        && m.Sender != currentUserId))
                .ToListAsync();

            // Returns empty list if the participant does not exists in any conversation.
            return userConversation;
        }

        public void UpdateMessagesAsChecked(V1Participant messages)
        {
            messages.IsRead = true;
        }

        public async Task MessageJob(string currentUserId)
        {
            try
            {
                _logger.LogInformation("Checking for new messages...");
                var newMessage = await GetNewMessages(currentUserId);

                // Are there any new messages?
                if (newMessage.Count > 0)
                {
                    // Sends a true bool value SignalR message if the user has at least one new message.
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", true);

                    foreach (var msg in newMessage)
                    {
                        // Commented out as the GUI currently updates IsRead values in the database when a conversation is opened.
                        // Update messages as checked in the database.
                        //UpdateMessagesAsChecked(msg);
                    }
                    // Update database with the new changes.
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Job ran successfully!");
                }
                else
                {
                    // Sends a false bool value SignalR message if the user has no new messages.
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", false);
                    _logger.LogWarning("No new messages for participant with conversationId: {currentUserId}.", currentUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking for messages.");
            }
        }
    }
}

