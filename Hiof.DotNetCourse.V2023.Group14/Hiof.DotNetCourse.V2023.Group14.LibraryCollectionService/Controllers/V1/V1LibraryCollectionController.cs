using System.Text.RegularExpressions;
using Azure.Core;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hiof.DotNetCourse.V2023.Group14.LibraryCollectionService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/libraries")]
    public class V1LibraryCollectionController : ControllerBase
    {
        private readonly LibraryCollectionContext _libraryCollectionContext;


        public V1LibraryCollectionController(LibraryCollectionContext libraryCollectionContext)
        {
            _libraryCollectionContext = libraryCollectionContext;
        }

        // Example of inserting a new object into the database. If you use swagger you can see that we supply it with a JSON DTO.
        // This Http request isn't coded to include lots of different Http codes yet.
        // Remember that it's important that this is set to async, along with await keywords.
        [HttpPost]
        [Route("entries")]
        public async Task<ActionResult> CreateEntry(V1LibraryEntry libraryEntry)
        {
            if (libraryEntry != null)
            {
                // Checks to see if there exists at least one ISBN number, and it is of adequate length.
                if (libraryEntry.LibraryEntryISBN13.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN10.IsNullOrEmpty())
                {
                    return BadRequest("The book you are trying to add does not have a valid ISBN");
                }

                if (!libraryEntry.LibraryEntryISBN10.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN10.Length != 10)
                {
                    return BadRequest("The ISBN10 of the book is of an invalid format.");
                } else if (!libraryEntry.LibraryEntryISBN13.IsNullOrEmpty() && libraryEntry.LibraryEntryISBN13?.Length != 13)
                {
                    return BadRequest("The ISBN13 of the book is of an invalid format.");
                }

                await _libraryCollectionContext.LibraryEntries.AddAsync(libraryEntry);
                await _libraryCollectionContext.SaveChangesAsync();

                return Ok();
            }
            
            return BadRequest("You failed to supply a valid library entry.");
        }

        [HttpGet("getLibraries")]

        public async Task<ActionResult> GetAllLibraries()
        {
            var libraries = from library in _libraryCollectionContext.LibraryEntries select library;

            if (libraries == null)
            {
                return NotFound("No libraries exist.");
            }
            else
            {
                return Ok(libraries);
            }
        }

        /*
        [HttpGet("getUsers")]

        public async Task<ActionResult> GetAllUsers()
        {
            
            var user = from u in _libraryCollectionContext.Users select u;
            if (user == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                return Ok(user);
            }

        }


        [HttpGet("getUserById")]

        public async Task<ActionResult> GetUserId(Guid guid)
        {
            var user = await _libraryCollectionContext.Users.FindAsync(guid);
            if (user == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                return Ok(user);
            }

        }


        [HttpGet("getUserByUserName")]

        public async Task<ActionResult> GetUserUserName(string userName)
        {
            var user = await _libraryCollectionContext.Users.Where(x => x.UserName.Contains(userName)).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                return Ok(user);
            }

        }

        [HttpGet("getUserByEmail")]

        public async Task<ActionResult> GetUserByEmail(string email)
        {
            var user = await _libraryCollectionContext.Users.Where(x => x.Email.Contains(email)).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                return Ok(user);
            }

        }

        [HttpPut("user")]
        public async Task<ActionResult> ChangeUserAccountUsingId( [FromBody] V1User user)
        {
            // In the event of a username or password being changed it's required to update both the 'users' and 'login_verification' tables.
            var existingUserData = await _libraryCollectionContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            var existingVerificationData = await _libraryCollectionContext.LoginModel.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUserData != null && existingVerificationData != null)
            {
                if (!Regex.IsMatch(user.Password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
                {
                    string msg = "Password must have at least one lower-case letter, one upper-case letter, one number, and one special character, and be at least 8 characters long";
                    return BadRequest(msg);
                } else if (existingUserData.Password != user.Password || existingUserData.UserName != user.UserName)
                {
                    // If the user updates their username or password the following code is executed.
                    // This is required because the password generates a unique salt in the 'login_verification' table as well.
                    var (hash, salt) = V1PasswordEncryption.Encrypt(user.Password);

                    var loginModel = new V1LoginModel();
                    loginModel.Id = existingVerificationData.Id;
                    loginModel.UserName = user.UserName;
                    loginModel.Token = existingVerificationData.Token;
                    loginModel.Password = hash;
                    loginModel.Salt = Convert.ToHexString(salt);

                    user.Password = hash;

                    _libraryCollectionContext.Entry<V1User>(existingUserData).CurrentValues.SetValues(user);
                    _libraryCollectionContext.SaveChanges();

                    _libraryCollectionContext.Entry<V1LoginModel>(existingVerificationData).CurrentValues.SetValues(loginModel);
                    _libraryCollectionContext.SaveChanges();
                } else
                {
                    _libraryCollectionContext.Entry<V1User>(existingUserData).CurrentValues.SetValues(user);
                    _libraryCollectionContext.SaveChanges();
                }
            }
            else
            {
                return NotFound("User doesn't exist");
            }
            return Ok();
        }
        
        // The following Http requests aren't necessary since we can use a single PUT requests to change any fields we wish.
        /*
        [HttpPut("ModifyEmail/{id}")]

        public async Task<ActionResult> ChangeEmail( string email, V1User user)
        {
            var existingData = await _userAccountContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id );
            if(existingData == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
               existingData.Email = email;
               _userAccountContext.SaveChanges();
            }
            return Ok();

        }

        [HttpPut("ChangeNames/{id}")]

        public async Task<ActionResult> ChangeNames(string firstName, string lastName, V1User user)
        {
            var existingData = await _userAccountContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingData == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                existingData.FirstName = firstName;
                existingData.LastName = lastName;
                _userAccountContext.SaveChanges();
            }
            return Ok();

        }

        [HttpPut("ChangeCity/{id}")]

        public async Task<ActionResult> ChangeCity(string city, V1User user)
        {
            var existingData = await _userAccountContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingData == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                existingData.City= city;
                _userAccountContext.SaveChanges();
            }
            return Ok();

        }

        [HttpDelete("deleteUser/{id}")]

        public async Task<ActionResult> DeleteUser(V1User user)
        {
            var existingUser = await _userAccountContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return NotFound(existingUser);
            }
            else
            {
                _userAccountContext.Users.Remove(existingUser);
                await _userAccountContext.SaveChangesAsync();
            }
            return Ok();
        }

        private static bool EmailIsValid(V1User user)
        {
            return Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$");
        }
        */
    }
}