using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers
{
    // Used to test the background jobs!
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        
        [HttpPost("fire")]
        public IActionResult FireAndForget(string mail)
        {
            // Stores the job id into a variable and adding a background job for SendMail method.
            var fireAndForgetJob = BackgroundJob.Enqueue(() => SendMail(mail));

            // Return OK (Status 200) with a message that includes the job id from the scheduled job
            return Ok($"Great! The job {fireAndForgetJob} has been completed. The mail has been sent to the user.");
        }

        [HttpGet("send-mail")]
        public void SendMail(string mail)
        {
            // Implement any logic you want - not in the controller but in some repository.
            Console.WriteLine($"This is a test - Hello {mail}");
        }
    }

}

