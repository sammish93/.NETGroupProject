using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserAccountController : ControllerBase
    {
        private readonly UserAccountContext _userAccountContext;

        public UserAccountController(UserAccountContext userAccountContext )
        {
            _userAccountContext = userAccountContext;
        }

        // Example of inserting a new object into the database. If you use swagger you can see that we supply it with a JSON DTO.
        // This Http request isn't coded to include lots of different Http codes yet.
        // Remember that it's important that this is set to async, along with await keywords.
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _userAccountContext.Users.AddAsync(user);
            //This line of code is blocked out because it was causing an error. I (Ashti) don't know why it was so for now it will be blocked out
            await _userAccountContext.SaveChangesAsync();
            return Ok();
        }
    }
}