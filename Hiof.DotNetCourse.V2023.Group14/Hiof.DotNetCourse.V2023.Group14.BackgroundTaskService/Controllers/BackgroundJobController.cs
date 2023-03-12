using System;
using System.Text;
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
        private readonly UserAccountContext _dbContext;

        public BackgroundJobController(UserAccountContext context)
        {
            _dbContext = context;
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult WelcomeMessage(string mail)
        {

            var jobId = BackgroundJob.Enqueue(() => SendWelcomeMail(mail));
            var message = $"Job ID: {jobId} has been completed. The mail has been sent to {mail}";

            return Ok(message);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckInactiveUsers()
        {
            RecurringJob.AddOrUpdate(() => CheckInactivity(), Cron.Daily());

            var message = "Reccuring job to check for inactive users daily is activated";
            return Ok(message);
        }

       

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public void CheckInactivity()
        {
            var inactiveTime = DateTime.Now.AddDays(-10);
            var inactiveUsers = _dbContext.Users.Where(u => u.LastActive < inactiveTime).ToList();
            CreateMailToInactiveUsers(inactiveUsers);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public void CreateMailToInactiveUsers(List<V1User> inactiveUsers)
        {
            foreach (var user in inactiveUsers)
            {
                StringBuilder message = new StringBuilder();
                message.Append($"\nUser {user.UserName} has been inactive for 10 days.");
                message.Append("\nPlease log in to the service soon.");

                Console.WriteLine($"Sending mail to: {user.Email}.");
                Console.WriteLine($"Content:\n{message}");
            }

        }

        public static void SendWelcomeMail(string mail)
        {
            Console.WriteLine($"\nWelcome {mail} to the Book Application!");
        }
    }

}

