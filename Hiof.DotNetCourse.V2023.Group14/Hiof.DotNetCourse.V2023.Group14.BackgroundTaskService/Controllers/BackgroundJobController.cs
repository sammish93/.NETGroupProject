﻿using Hangfire;
using Hangfire.Storage;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
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

        public BackgroundJobController(IHttpClientFactory httpClientFactory, ILogger<BackgroundJobController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
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
        public IActionResult StartJob()
        {
            UpdateCacheJob job = new(_httpClient);
            try
            {
                // This will make the job run every hour on the first minute.
                RecurringJob.AddOrUpdate(() => job.Update(), "0 * * * *");
                return Ok("Cache updating job is scheduled!");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error: An Exception occurred!", ex.Message);
                return StatusCode(500, "An error occurred while scheduling the cache updating job.");
            }
        }

        [HttpPost]
        [Route("InactiveUser/[action]")]
        public IActionResult Start()
        {
            InactiveUsers inactive = new(_httpClient);
            try
            {
                // This will check the database for inactive users every day.
                RecurringJob.AddOrUpdate(() => inactive.CheckInactivity(), Cron.Daily());

                var message = "Recurring job to check for inactive users daily is activated";
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
        public IActionResult Stop()
        {
            var storage = JobStorage.Current;
            var recurringJobIds = storage.GetConnection().GetRecurringJobs().Select(x => x.Id);

            if (recurringJobIds.Contains("BackgroundJobController.CheckInactivity"))
            {
                RecurringJob.RemoveIfExists("BackgroundJobController.CheckInactivity");
                return Ok("Inactive user-job successfully stopped.");
            }
            else
            {
                return NotFound("Inactive user-job not found.");
            }
        }

        [HttpPost]
        [Route("MessageChecker/[action]")]
        public IActionResult Start(string userId)
        {
            MessageChecker messageCheckerJob = new();
            try
            {
                messageCheckerJob.CheckMessages(userId);
                return Ok("Job successfully created!");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Error: An Exception occurred!", ex.Message);
                return StatusCode(500, "An error occurred while scheduling the message checker job.");
            }

        }
       
        public static void SendWelcomeMail(string mail)
        {
            Console.WriteLine($"\nWelcome {mail} to the Book Application!");
        }
    }

}

