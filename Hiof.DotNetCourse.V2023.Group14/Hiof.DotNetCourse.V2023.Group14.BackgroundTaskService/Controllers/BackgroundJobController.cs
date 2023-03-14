﻿using System;
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
        private readonly UserAccountContext _dbContext;

        public BackgroundJobController(UserAccountContext context)
        {
            _dbContext = context;
        }


        [HttpPost]
        [Route("WelcomeMessage/[action]")]
        public IActionResult Run(string mail)
        {

            var jobId = BackgroundJob.Enqueue(() => SendWelcomeMail(mail));
            var message = $"Job ID: {jobId} has been completed. The mail has been sent to {mail}";

            return Ok(message);
        }

        // TODO: This method is causing circular references and needs to be fixed.
        [HttpPost]
        [Route("UpdateCache/[action]")]
        public IActionResult StartJob()
        {
            // This will make the job run every 5 minute.
            RecurringJob.AddOrUpdate(() => UpdateCacheJob.Update(_dbContext), "*/5 * * * *");

            return Ok("Cache updating job is scheduled!");
        }

        [HttpPost]
        [Route("InactiveUser/[action]")]
        public IActionResult Start()
        {
            // This will check the database for inactive users every day.
            RecurringJob.AddOrUpdate(() => CheckInactivity(), Cron.Daily());

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

                var result = $"\nMail sent to: {user.Email}.";
                var content = $"\nContent: {message}";
                WriteResultToFile(result, content);
            }

        }

        private static void WriteResultToFile(string result, string content)
        {
            using StreamWriter file = new("TextFile/log.txt", append: true);
            file.Write(result + content);
        }

        public static void SendWelcomeMail(string mail)
        {
            Console.WriteLine($"\nWelcome {mail} to the Book Application!");
        }
    }

}

