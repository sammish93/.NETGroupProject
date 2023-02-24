using System;
using Hangfire;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
	public class Messages
	{
		private readonly ILogger<Messages> _logger;

		public Messages(ILogger<Messages> logger)
		{
			_logger = logger;
		}

		public void CheckForNewMessages(string messageType, int interval)
		{
            _logger.LogInformation($"Background job for checking {messageType} every hour.");

            // This will run the job on minute '0' every hour.
			RecurringJob.AddOrUpdate(() => MessageJob(messageType), cronExpression: "0 * * * *");
		}

        public async Task MessageJob(string messageType)
        {
            try
            {
                // Check for messages of the specified type
                // TODO: Implement logic for checking messages

                _logger.LogInformation($"Checking for {messageType} messages...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking for {messageType} messages.");
            }
        }
    }
}

