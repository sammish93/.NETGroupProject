using System.ComponentModel.DataAnnotations;
using Hangfire;
using Hangfire.Storage;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundJobController : ControllerBase
    {
        // Dependency injection - must update the raport.
        private readonly HttpClient _httpClient;
        private readonly ILogger<BackgroundJobController> _logger;
        private readonly MessageChecker _messageChecker;

        public BackgroundJobController(IHttpClientFactory httpClientFactory, ILogger<BackgroundJobController> logger, MessageChecker messageChecker)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _messageChecker = messageChecker;
        }


        [HttpPost]
        [Route("WelcomeMessage/[action]")]
        public IActionResult Run(string mail)
        {

            var jobId = BackgroundJob.Enqueue(() => SendWelcomeMail(mail));
            var message = $"Job ID: {jobId} has been completed. The mail has been sent to {mail}";

            return Ok(message);
        }

        [HttpPost]
        [Route("UpdateCache/[action]")]
        public IActionResult StartUpdateCacheJob([Required] string jobId)
        {
            UpdateCacheJob job = new(_httpClient);
            try
            {
                // This will make the job run every hour on the first minute.
                RecurringJob.AddOrUpdate(jobId, () => job.Update(), "0 * * * *");
                return Ok("Cache updating job is scheduled!");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error: An Exception occurred!", ex.Message);
                return StatusCode(500, "An error occurred while scheduling the cache updating job.");
            }
        }

        [HttpDelete]
        [Route("UpdateCache/[action]")]
        public IActionResult StopUpdateCacheJob([Required] string jobId)
        {
            var storage = JobStorage.Current;
            var recurringJobIds = storage.GetConnection().GetRecurringJobs().Select(x => x.Id);

            if (recurringJobIds.Contains(jobId))
            {
                RecurringJob.RemoveIfExists(jobId);
                return Ok($"Update cache job successfully stopped. Job ID: {jobId}.");
            }
            else
            {
                return NotFound("Update cache job not found.");
            }
        }

        [HttpPost]
        [Route("InactiveUser/[action]")]
        public IActionResult StartInactiveJob([Required] string jobId)
        {
            InactiveUsers inactive = new(_httpClient);
            try
            {
                // This will check the database for inactive users every day.
                RecurringJob.AddOrUpdate(jobId, () => inactive.CheckInactivity(), Cron.Daily());

                var message = $"Recurring job to check for inactive users daily is activated. Job ID: {jobId}.";
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error: An Exception occurred!", ex.Message);
                return StatusCode(500, "An error occurred while scheduling the check for inactive users daily job.");
            }
            
        }


        [HttpDelete]
        [Route("InactiveUser/[action]")]
        public IActionResult StopInactiveJob([Required] string jobId)
        {
            var storage = JobStorage.Current;
            var recurringJobIds = storage.GetConnection().GetRecurringJobs().Select(x => x.Id);

            if (recurringJobIds.Contains(jobId))
            {
                RecurringJob.RemoveIfExists(jobId);
                return Ok($"Inactive user-job with ID: {jobId} successfully stopped.");
            }
            else
            {
                return NotFound("Inactive user-job not found.");
            }
        }

        [HttpPost]
        [Route("MessageChecker/[action]")]
        public IActionResult StartMessageJob([Required] string userId)
        {
            try
            {
                _logger.LogInformation("Recurring message checking job started for user ID: {userId}.", userId);
                _messageChecker.CheckMessages(userId);
                return Ok($"Recurring message checking job successfully created for user ID: {userId}.");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error: An Exception occurred!", ex.Message);
                return StatusCode(500, "An error occurred while scheduling the message checker job.");
            }
        }

        [HttpDelete]
        [Route("MessageChecker/[action]")]
        public IActionResult StopMessageJob([Required] string userId)
        {
            try
            {
                var storage = JobStorage.Current;
                var recurringJobIds = storage.GetConnection().GetRecurringJobs().Select(x => x.Id);

                if (recurringJobIds.Contains(userId))
                {
                    _logger.LogInformation("Successfully removed message checking job.");
                    RecurringJob.RemoveIfExists(userId);
                    return Ok($"Message checking job successfully stopped for user ID: {userId}.");
                }
                else
                {
                    _logger.LogWarning("No message checking job found for user ID: {userId}.", userId);
                    return NotFound($"No message checking job found for user ID: {userId}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error: An Exception occurred!", ex.Message);
                return StatusCode(500, "An error occurred while stopping the message checker job.");
            }
        }

        [HttpGet]
        [Route("MessageChecker/NewMessages/{userId}")]
        public async Task<IActionResult> GetNewMessages([Required] string userId)
        {
            var newMessages = await _messageChecker.GetNewMessages(userId);
            if (!newMessages.Any())
            {
                _logger.LogWarning("No new messages found for user with ID: {userId}.", userId);
                return NotFound($"No new messages for user with ID: {userId}.");
            }
            else
            {
                _logger.LogInformation("New messages found for user with ID: {userId}.", userId);
                return Ok(newMessages);
            }
        }

       
        public static void SendWelcomeMail(string mail)
        {
            Console.WriteLine($"\nWelcome {mail} to the Book Application!");
        }
    }

}

