using System;
using System.Text.RegularExpressions;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
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

		/// <summary>
		/// Method that checks if the username and password from the POST
		/// request match with what is stored in the database.
		/// </summary>
		/// <param name="user">
		/// Used to get username and password from a field/form.
		/// </param>
		/// <returns>
		/// On success: 'Login successful', else 'Invalid Login Attempt'.
		/// </returns>
		[HttpPost]
		public ActionResult<string> VerifyLogin([FromBody] User user)
		{
			var validationResult = InputValidation(user);

			// Check if fields are empty or null.
			if (validationResult != null)
			{
				return validationResult;
			}

			// Check if length is within the limits.
			if (!CheckLength(user))
			{
				string msg = "Username and password length must be between: ";
                return BadRequest(msg + Min + "-" + Max);
			}

			if (!CheckCharacters(user))
			{
				return BadRequest("Only alphanumeric characters in username");
			}

			// Check if name and password match with database.
			if (user.UserName != "DB info" && user.Password != "DB info")
			{
                return Unauthorized("Invalid Login Attempt");
            }
			else
			{
                return Ok("Login successful");
            }
		}

        /// <summary>
        /// Method that checks if the results from the POST-request
        /// is not null or empty.
        /// </summary>
        /// <param name="user">
        /// Used to get username and password from a field/form.
		/// </param>
        /// <returns>
		/// Null on good request, errormessage on bad.
		/// </returns>
        private ActionResult? InputValidation(User user)
		{
			if (string.IsNullOrEmpty(user.UserName) ||
				string.IsNullOrEmpty(user.Password))
			{
				return BadRequest("Username and password are required!");
			}
			
			return null;
		}

        /// <summary>
        /// Checks if the length of both username and password are within
        /// the specified limits.
        /// </summary>
        /// <param name="user">
        /// Used to get username and password from a field/form.
        /// </param>
        /// <returns>
		/// True if they are within the limits, false otherwise.
		/// </returns>
        private static bool CheckLength(User user)
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

        /// <summary>
        /// Checks if the username is written with alphanumeric characters.
        /// </summary>
        /// <param name="user">
        /// Used to get username a field/form.
        /// </param>
        /// <returns>
        /// False if it contains non-alphanumeric characters, true else.
        /// </returns>
        private static bool CheckCharacters(User user)
		{
            return Regex.IsMatch(user.UserName, @"^[a-zA-Z0-9]+$");
        }
	}
}

