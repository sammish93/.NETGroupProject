using System;
using System.Text.RegularExpressions;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Safety;
using Hiof.DotNetCourse.V2023.Group14.LoginService.Data;
using Microsoft.AspNetCore.Mvc;


namespace Hiof.DotNetCourse.V2023.Group14.LoginService.Controllers
{

	[ApiController]
	[Route("login")]
	public class LoginController : ControllerBase
	{
		// Constants used for input validation.
		private const int Min = 5;
		private const int Max = 20;

		private readonly ILogger<LoginController> _logger;

		public LoginController(ILogger<LoginController> logger)
		{
			_logger = logger;
		}

		
		 // Method that checks if the username and password from the POST
		 // request match with what is stored in the database.

		[HttpPost]
		public ActionResult<string> VerifyLogin([FromBody] LoginInfo user)
		{
			var validationResult = InputValidation(user);

			// Check if fields are empty or null.
			if (validationResult != null)
			{
				return validationResult;  
			} 

			if (!CheckLength(user))
			{
				string msg = "Username and password length must be between: ";
                return BadRequest(msg + Min + "-" + Max);
			}

			if (!CheckCharacters(user))
			{
				return BadRequest("Only alphanumeric characters in username");
			}

			if (user.UserName != "steezBrah" || user.Password != "abc123")
			{
                return Unauthorized("Invalid Login Attempt");
            }
			else
			{
				var token = Token.createToken(user.Id);

				// Add token in HTTP headers so that the client can include
				// this in all subsequent requests.
				Response.Headers.Add("Autorization", "Bearer" + token);

                return Ok("Login Success");
            }
		}

        // Method that checks if the results from the POST-request
        // is not null or empty.

        private ActionResult? InputValidation(LoginInfo user)
		{
			if (string.IsNullOrEmpty(user.UserName) ||
				string.IsNullOrEmpty(user.Password))
			{
				return BadRequest("Username and password are required!");
			}
			
			return null;
		}


        // Checks if the length of both username and password are within
        // the specified limits.

        private static bool CheckLength(LoginInfo user)
		{
            if (user.UserName.Length < Min || user.Password.Length < Min)
            {
				return false;
            }

            if (user.UserName.Length > Max || user.Password.Length > Max)
            {
				return false;
            }

			return true;

        }


        // Checks if the username is written with alphanumeric characters.

        private static bool CheckCharacters(LoginInfo user)
		{
            return Regex.IsMatch(user.UserName, @"^[a-zA-Z0-9]+$");
        }
	}
}


