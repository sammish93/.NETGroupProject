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
        public async Task<IEnumerable<V1Messages>> GetNewMessages(string currentUserId)
        {
            // Get conversation where current user is a participant.
            var userConversation = await _context.Participant
                .Where(p => p.Participant == currentUserId)
                .Select(p => p.ConversationId)
                .ToListAsync();

            // Get the new messages for those conversations.
            var messages = await _context.Messages
                .Where(m => userConversation.Contains(m.ConversationId) && !m.IsChecked && m.Sender != currentUserId)
                .ToListAsync();

            // Return empty list if messages is null.
            return messages ?? Enumerable.Empty<V1Messages>();
        }

        public async Task UpdateMessagesAsChecked(IEnumerable<V1Messages> messages)
        {
            foreach (var message in messages)
            {
                message.IsChecked = true;
            }
            await _context.SaveChangesAsync();

        }

        public async Task MessageJob(string currentUserId)
        {
            try
            {
                _logger.LogInformation("Checking for new messages...");

                var newMessage = await GetNewMessages(currentUserId);

                if (newMessage.Any())
                {
                    // Update messages as checked in the database.
                    await UpdateMessagesAsChecked(newMessage);

                }

                _logger.LogInformation("Job ran successfully!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking for messages.");
            }
        }
    }
}

