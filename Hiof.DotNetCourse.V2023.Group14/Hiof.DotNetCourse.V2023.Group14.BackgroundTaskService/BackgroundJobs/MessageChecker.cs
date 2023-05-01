using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.DTO.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;

// This class needs to be modified later in order to work.

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
  	public class MessageChecker
	{
		private readonly ILogger<MessageChecker> _logger;
        private readonly MessagingContext _context;
        private readonly MessageToMaui _messageToMaui;

		public MessageChecker(ILogger<MessageChecker> logger, MessagingContext context, MessageToMaui messageToMaui)
		{
			_logger = logger;
            _context = context;
            _messageToMaui = messageToMaui;
            
		}

        public void CheckMessages()
		{
            _logger.LogInformation("Background job for checking messages every 5 secounds.");
			RecurringJob.AddOrUpdate(() => MessageJob(), cronExpression: "*/5 * * * * *");
		}

        public async Task<IEnumerable<V1Messages>> GetNewMessages()
        {
            return await _context.Messages
                .Where(m => !m.IsChecked)
                .ToListAsync();
        }

        public async Task UpdateMessagesAsChecked(IEnumerable<V1Messages> messages)
        {
            foreach (var message in messages)
            {
                message.IsChecked = true;
            }
            await _context.SaveChangesAsync();

        }

        public async Task MessageJob()
        {
            try
            {
                _logger.LogInformation("Checking for new messages...");

                var newMessage = await GetNewMessages();

                if (newMessage.Any())
                {
                    // Send the new message to maui gui.
                    _messageToMaui.OnNewMessagesReceived(newMessage.ToList());

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

