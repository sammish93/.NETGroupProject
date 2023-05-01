using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;

// This class needs to be modified later in order to work.

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
    // Background job used to check for new messages in a messaging system. The
    // class uses the Hangfire library to create a background job that runs every
    // 10 seconds. The CheckMessage method adds the MessageJob method to the
    // background job, which is responsible for checking for new messages.
  
	public class MessageChecker
	{
		private readonly ILogger<MessageChecker> _logger;
        private readonly MessagingContext _context;

		public MessageChecker(ILogger<MessageChecker> logger, MessagingContext context)
		{
			_logger = logger;
            _context = context;
		}

        // This will make the job run every 10 second.
        public void CheckMessages()
		{
            _logger.LogInformation("Background job for checking messages every hour.");
			RecurringJob.AddOrUpdate(() => MessageJob(), cronExpression: "*/10 * * * * *");
		}

        public async Task<IEnumerable<V1Messages>> GetNewMessagesAsync()
        {
            throw new NotImplementedException();

        }

        public async Task MessageJob()
        {
            try
            {
                _logger.LogInformation("Checking for new messages...");

                // check for mesages in the database here.

                _logger.LogInformation("Job ran successfully!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking for messages.");
            }
        }
    }
}

