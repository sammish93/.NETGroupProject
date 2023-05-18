using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.DTO.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
  	public class MessageChecker
	{
		private readonly ILogger<MessageChecker> _logger;
        private readonly MessagingContext _context;

		public MessageChecker(ILogger<MessageChecker> logger, MessagingContext context)
		{
			_logger = logger;
            _context = context;
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
                .Where(p => p.Participant == currentUserId && !p.IsRead)
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
                    foreach (var msg in newMessage)
                    {
                        // Update messages as checked in the database.
                        UpdateMessagesAsChecked(msg);
                    }
                    // Update database with the new changes.
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Job ran successfully!");
                }
                else
                {
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

