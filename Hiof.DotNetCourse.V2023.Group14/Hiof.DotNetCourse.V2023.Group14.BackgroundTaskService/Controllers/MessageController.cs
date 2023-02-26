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
        private readonly ILogger<MessageController> _logger;
        private readonly HttpClient _httpClient;

        public MessageController(ILogger<MessageController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpPost("trigger-message-check")]
        public IActionResult TriggerMessageCheck()
        {
            try
            {
                // Enqueue the Hangfire job to check for new messages
                BackgroundJob.Enqueue(() => new MessageChecker((ILogger<MessageChecker>)_logger, _httpClient).MessageJob());

                return Ok("Message check triggered.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error triggering message check.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}

