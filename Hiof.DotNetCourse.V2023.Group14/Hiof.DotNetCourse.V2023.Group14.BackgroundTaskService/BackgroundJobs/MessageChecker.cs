using System;
using Hangfire;

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
        private readonly HttpClient _httpClient;

		public MessageChecker(ILogger<MessageChecker> logger, HttpClient client)
		{
			_logger = logger;
            _httpClient = client;
		}

        // This will make the job run every 10 second.
        public void CheckMessages()
		{
            _logger.LogInformation("Background job for checking messages every hour.");
			RecurringJob.AddOrUpdate(() => MessageJob(), cronExpression: "*/10 * * * * *");
		}

        public async Task MessageJob()
        {
            try
            {
                _logger.LogInformation("Checking for new messages...");

                // Retrieve all conversations.
                HttpResponseMessage response = await _httpClient.GetAsync("/messages");
                response.EnsureSuccessStatusCode();

                // TODO: Process the response and send notification if there are new messages.
                // Not really sure what to do here yet. Maybe we need to wait to the messaging
                // is up and running.

                _logger.LogInformation("Job ran successfully!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking for messages.");
            }
        }
    }
}

