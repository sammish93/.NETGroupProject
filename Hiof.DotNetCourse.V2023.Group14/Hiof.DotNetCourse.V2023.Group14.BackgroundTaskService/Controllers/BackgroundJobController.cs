using System;
using Hangfire;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundJobController : ControllerBase
    {
        private readonly InactiveUsers _inactiveUsers;
        private readonly UserAccountContext _dbContext;

        public BackgroundJobController(UserAccountContext context, InactiveUsers users)
        {
            _dbContext = context;
            _inactiveUsers = users;
         
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult WelcomeMessage(string mail)
        {

            var jobId = BackgroundJob.Enqueue(() => SendMail(mail));
            var message = $"Job ID: {jobId} has been completed. The mail has been sent to {mail}";

            return Ok(message);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckInactiveUsers()
        {
            RecurringJob.AddOrUpdate(() => _inactiveUsers.CheckInactiveUsers(_dbContext), Cron.Daily());

            var message = "Reccuring job to check for inactive users daily is activated";
            return Ok(message);
        }



        public static void SendMail(string mail)
        {
            Console.WriteLine($"Welcome {mail} to the Book Application!");
        }
    }

}

