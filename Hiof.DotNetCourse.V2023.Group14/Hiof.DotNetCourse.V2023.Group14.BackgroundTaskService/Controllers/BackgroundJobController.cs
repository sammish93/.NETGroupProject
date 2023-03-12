using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundJobController : ControllerBase
    {

        [HttpPost]
        [Route("[action]")]
        public IActionResult SendWelcomeMessage(string mail)
        {

            var jobId = BackgroundJob.Enqueue(() => SendMail(mail));

            return Ok($"Great! The job {jobId} has been completed. The mail has been sent to the user.");
        }

        public void SendMail(string mail)
        {
            Console.WriteLine($"This is a test - Hello {mail}");
        }
    }

}

