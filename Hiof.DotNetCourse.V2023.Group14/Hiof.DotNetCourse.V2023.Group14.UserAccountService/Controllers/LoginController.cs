using System;
using System.Linq;
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
        private readonly LoginDbContext _contex;

        
        public LoginController(LoginDbContext context)
        {
            _contex = context;
        }
        
        // Method that checks if the username and password from the POST
        // request match with what is stored in the database.

        [HttpPost("verification")]
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

            // Retrieve user with the same id from database.
            var dbUser = GetDbUser(user.Id);

            if (dbUser != null && dbUser.Salt != null)
            {
                // Get the hashed password and salt from database.
                var salt = Convert.FromBase64String(dbUser.Salt);
                var hash = dbUser.Password;

                if (salt != null && hash != null)
                {
                    // Check if the hashed password and salt from database
                    // match with the users password.
                    bool result = PasswordEncryption.Verify(user.Password, hash, salt);

                    if (user.UserName != dbUser.UserName || result != true)
                    {
                        return Unauthorized("Invalid Login Attempt");
                    }

                    return Ok("Login Success");
                }
            }

            return Unauthorized("Account does not exists.");
        }

        // Gets an entity from the database based on id.

        private LoginModel GetDbUser(int id)
        {
            return _contex.LoginModel.Single(l => l.Id == id);
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
