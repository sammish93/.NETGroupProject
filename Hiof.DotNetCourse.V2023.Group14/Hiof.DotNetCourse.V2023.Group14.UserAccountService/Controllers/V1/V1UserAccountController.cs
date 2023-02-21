using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0")]
    public class V1UserAccountController : ControllerBase
    {
        private readonly UserAccountContext _userAccountContext;

        public V1UserAccountController(UserAccountContext userAccountContext )
        {
            _userAccountContext = userAccountContext;
        }

        // Example of inserting a new object into the database. If you use swagger you can see that we supply it with a JSON DTO.
        // This Http request isn't coded to include lots of different Http codes yet.
        // Remember that it's important that this is set to async, along with await keywords.
        [HttpPost]
        [Route("users")]
        public async Task<ActionResult> Create(V1User user)
        {
            await _userAccountContext.Users.AddAsync(user);
            //This line of code is blocked out because it was causing an error. I (Ashti) don't know why it was so for now it will be blocked out
            await _userAccountContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetUserById")]

        public async Task<ActionResult> GetUserId(Guid guid)
        {
            V1User user = await _userAccountContext.Users.FindAsync(guid);
            if(user == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                return Ok(user);
            }
            
        }


        [HttpGet("GetUserByUserName")]

        public async Task<ActionResult> GetUserUserName(string userName)
        {
            V1User user = await _userAccountContext.Users.Where(x => x.UserName.Contains(userName)).FirstOrDefaultAsync();
            
            if (user == null)
            {
                return NotFound("User doesn't exist");
            }
            else
            {
                return Ok(user);
            }

        }

    }
}