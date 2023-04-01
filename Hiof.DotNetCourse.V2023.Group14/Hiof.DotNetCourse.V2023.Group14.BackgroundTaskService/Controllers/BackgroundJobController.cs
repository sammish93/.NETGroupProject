using System;
using System.Text;
using System.Text.Json.Serialization;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundJobController : ControllerBase
    {
        // Dependency injection - must update the raport.
        private readonly HttpClient _httpClient;

        public BackgroundJobController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
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
            UpdateCacheJob job = new UpdateCacheJob(_httpClient);
           
            // This will make the job run every hour on the first minute.
            RecurringJob.AddOrUpdate(() => job.Update(), "0 * * * *");

            return Ok("Cache updating job is scheduled!");
        }

        [HttpPost]
        [Route("InactiveUser/[action]")]
        public IActionResult Start()
        {
            InactiveUsers inactive = new InactiveUsers(_httpClient);

            // This will check the database for inactive users every day.
            RecurringJob.AddOrUpdate(() => inactive.CheckInactivity(), Cron.Daily());

            var message = "Recurring job to check for inactive users daily is activated";
            return Ok(message);
        }

        [HttpPost]
        [Route("InactiveUser/[action]")]
        public IActionResult Stop()
        {
            RecurringJob.RemoveIfExists("BackgroundJobController.CheckInactivity");
            return Ok("InactiveUser-Job successfully stopped.");
        }
       
        public static void SendWelcomeMail(string mail)
        {
            Console.WriteLine($"\nWelcome {mail} to the Book Application!");
        }
    }

}

