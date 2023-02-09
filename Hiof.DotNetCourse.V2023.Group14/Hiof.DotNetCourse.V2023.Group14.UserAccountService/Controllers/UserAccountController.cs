using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAccountController : Controller
    {
        private readonly UserAccountContext _userAccountContext;

        public UserAccountController(UserAccountContext userAccountContext)
        {
            _userAccountContext = userAccountContext;
        }
        [HttpGet]
        public async Task<ActionResult> Create(User user)
        {
            await _userAccountContext.Users.AddAsync(user);
            await _userAccountContext.SaveChangesAsync();
            return Ok();
        }
    }
}
