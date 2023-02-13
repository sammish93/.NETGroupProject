using System;
using System.Text.RegularExpressions;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Security;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.LoginService.Controllers
{

    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        // Constants used for input validation.
        private const int Min = 5;
        private const int Max = 20;

        private readonly LoginDbContext _loginDbContext;

        public LoginController(LoginDbContext context)
        {
            _loginDbContext = context;
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

            if (user.UserName != "stian" || user.Password != "hello23")
            {
                return Unauthorized("Invalid Login Attempt");
            }
            else
            {
                var encryption = new PasswordEncryption();
                var (hash, salt) = encryption.Encrypt(user.Password);

                var dbUser = new LoginInfo(user.Id, user.UserName, (hash + Convert.ToHexString(salt)));
                dbUser.Token = Token.CreateToken(user.Id);

                // Add token in HTTP headers so that the client can include
                // this in all subsequent requests.
                Response.Headers.Add("Autorization", "Bearer" + dbUser.Token);

                return Ok("Login Success");
            }
        }

        // Returns all the users in the database as a list.
        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _loginDbContext.LoginInfo.ToListAsync();
            return Ok(users);
        }

        // Returns user from database specified by Id.
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _loginDbContext.LoginInfo.FirstOrDefaultAsync(u => u.Id == id);

            if (user is not null)
            {
               return Ok(user);
            }
            return NotFound();
            
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
