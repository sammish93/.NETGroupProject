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
        public IActionResult WelcomeMessage(string mail)
        {

            var jobId = BackgroundJob.Enqueue(() => SendMail(mail));
            var message = $"Job ID: {jobId} has been completed. The mail has been sent to {mail}";

            return Ok(message);
        }

        public static void SendMail(string mail)
        {
            Console.WriteLine($"Welcome {mail} to the Book Application!");
        }
    }

}

