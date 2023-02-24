﻿using System.Text.RegularExpressions;
using Azure.Core;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.Security;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/users")]
    public class V1UserAccountController : ControllerBase
    {
        private readonly UserAccountContext _userAccountContext;


        public V1UserAccountController(UserAccountContext userAccountContext)
        {
            _userAccountContext = userAccountContext;
        }

        // Example of inserting a new object into the database. If you use swagger you can see that we supply it with a JSON DTO.
        // This Http request isn't coded to include lots of different Http codes yet.
        // Remember that it's important that this is set to async, along with await keywords.
        [HttpPost]
        [Route("user")]
        public async Task<ActionResult> Create(V1User user)
        {
            if(user != null)
            {
                var (hash, salt) = V1PasswordEncryption.Encrypt(user.Password);
                user.Password = hash;
                V1LoginModel loginModel = new V1LoginModel(user.Id, user.UserName, hash, Convert.ToHexString(salt));
                loginModel.Token = V1Token.CreateToken(loginModel.Id);

                await _userAccountContext.Users.AddAsync(user);
                await _userAccountContext.SaveChangesAsync();

                await _userAccountContext.LoginModel.AddAsync(loginModel);
                await _userAccountContext.SaveChangesAsync();

            }
            
            return Ok();
        }

        [HttpGet("getUsers")]

        public async Task<ActionResult> GetAllUsers()
        {
            
            var user = from u in _userAccountContext.Users select u;
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
            var user = await _userAccountContext.Users.FindAsync(guid);
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
            var user = await _userAccountContext.Users.Where(x => x.UserName.Contains(userName)).FirstOrDefaultAsync();

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
            var user = await _userAccountContext.Users.Where(x => x.Email.Contains(email)).FirstOrDefaultAsync();

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
            var existingUserData = await _userAccountContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            var existingVerificationData = await _userAccountContext.LoginModel.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUserData != null && existingVerificationData != null)
            {
                // If the user updates their username or password the following code is executed.
                // This is required because the password generates a unique salt in the 'login_verification' table as well.
                if (existingUserData.Password != user.Password || existingUserData.UserName != user.UserName)
                {
                    var (hash, salt) = V1PasswordEncryption.Encrypt(user.Password);

                    var loginModel = new V1LoginModel();
                    loginModel.Id = existingVerificationData.Id;
                    loginModel.UserName = user.UserName;
                    loginModel.Token = existingVerificationData.Token;
                    loginModel.Password = hash;
                    loginModel.Salt = Convert.ToHexString(salt);

                    user.Password = hash;

                    _userAccountContext.Entry<V1User>(existingUserData).CurrentValues.SetValues(user);
                    _userAccountContext.SaveChanges();

                    _userAccountContext.Entry<V1LoginModel>(existingVerificationData).CurrentValues.SetValues(loginModel);
                    _userAccountContext.SaveChanges();
                } else
                {
                    _userAccountContext.Entry<V1User>(existingUserData).CurrentValues.SetValues(user);
                    _userAccountContext.SaveChanges();
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