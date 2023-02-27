using System.Text.RegularExpressions;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers.V1
{

    [ApiController]
    [Route("api/1.0/login")]
    public class V1LoginController : ControllerBase
    {
        // Constants used for input validation.
        // private const int Min = 5;
        // private const int Max = 20;
        private readonly LoginDbContext _context;

        
        public V1LoginController(LoginDbContext context)
        {
            _context = context;
        }

        // Method that checks if the username and password from the POST
        // request match with what is stored in the database.

        [HttpPost]
        [Route("verification")]
        public async Task<IActionResult> VerifyLogin([FromBody] V1LoginInfo user)
        {
            // Check if fields are empty or null.
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Both username and password are required.");
            } else if (!V1User.ValidUsername(user.UserName))
            {
                return BadRequest("Username must be between 5 and 20 characters and only contain alphanumerical characters.");
            }

            // Retrieve user with the same id from database.
            var dbUser = await GetDbUser(user.UserName);

            if (dbUser != null && dbUser.Salt != null)
            {
                // Get the hashed password and salt from database.
                var salt = Convert.FromHexString(dbUser.Salt);
                var hash = dbUser.Password;

                if (salt != null && hash != null)
                {
                    // Check if the hashed password and salt from database
                    // match with the users password.
                    bool result = V1PasswordEncryption.Verify(user.Password, hash, salt);

                    if (user.UserName != dbUser.UserName || result != true)
                    {
                        return Unauthorized("Invalid Login Attempt.");
                    }

                    return Ok("Login Success.");
                }
            }

            return Unauthorized("Account does not exist.");
        }

        // Gets an entity from the database based on id.
        private async Task<V1LoginModel?> GetDbUser(string username)
        {
            try
            {
                return await _context.LoginModel.SingleOrDefaultAsync(l => l.UserName == username);

            }
            catch (InvalidOperationException)
            {
                return null;
            }

        }

        // Commented out for security reasons. This request shouldn't be publicly allowed to be edited. We can allow those with ADMIN privileges to call this later on with a valid token if we wish.
        /*
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(V1LoginModel loginModel)
        {
            if (loginModel != null)
            {
                loginModel.Token = V1Token.CreateToken(loginModel.Id);
                await _context.LoginModel.AddAsync(loginModel);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        // Method that checks if the results from the POST-request
        // is not null or empty.
        private bool InputValidation(V1LoginInfo user)
        {
            if (string.IsNullOrEmpty(user.UserName) ||
                string.IsNullOrEmpty(user.Password))
            {
                return true;
            }

            return false;


        // Checks if the length of both username and password are within
        // the specified limits.

        private static bool CheckLength(V1LoginInfo user)
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
        */
    }
}
