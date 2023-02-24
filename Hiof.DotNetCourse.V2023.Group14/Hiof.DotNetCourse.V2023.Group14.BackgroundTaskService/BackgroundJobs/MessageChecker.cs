using System;
using Hangfire;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs
{
    // To use this class in pracice, we need to add an instanse of the class
    // in the constructor of the class that want to use it.

	public class MessageChecker
	{
		private readonly ILogger<MessageChecker> _logger;

		public MessageChecker(ILogger<MessageChecker> logger)
		{
			_logger = logger;
		}

        // This will run the job on minute '0' every hour.
        public void CheckMessages(string messageType)
		{
            _logger.LogInformation($"Background job for checking {messageType} every hour.");
			RecurringJob.AddOrUpdate(() => MessageJob(messageType), cronExpression: "0 * * * *");
		}

        public async Task MessageJob(string messageType)
        {
            try
            {
                // Check for messages of the specified type
                // TODO: Implement logic for checking messages

                // Here we can send requests to other microservices
                // to get the messages we want.
                
                _logger.LogInformation($"Checking for {messageType} messages...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking for {messageType} messages.");
            }
        }
    }
}

