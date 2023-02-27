using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        // This is just a test method, it will be removed later.
        [HttpPost("fire")]
        public IActionResult FireAndForget(string mail)
        {
            // Stores the job id into a variable and adding a background job for SendMail method.
            var fireAndForgetJob = BackgroundJob.Enqueue(() => SendMail(mail));

            return Ok($"Great! The job {fireAndForgetJob} has been completed. The mail has been sent to the user.");
        }

        [HttpGet("send-mail")]
        public void SendMail(string mail)
        {
            Console.WriteLine($"This is a test - Hello {mail}");
        }
    }

}

