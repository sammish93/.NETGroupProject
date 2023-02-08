using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly DbOrmTestClassContext _dbOrmTestClassContext;

        public TestController(DbOrmTestClassContext dbOrmTestClassContext)
        {
            _dbOrmTestClassContext = dbOrmTestClassContext;
        }

        // Example of inserting a new object into the database. If you use swagger you can see that we supply it with a JSON DTO.
        // This Http request isn't coded to include lots of different Http codes yet.
        // Remember that it's important that this is set to async, along with await keywords.
        [HttpPost]
        public async Task<ActionResult> Create(DbOrmTestClass dbOrmTestClass)
        {
            await _dbOrmTestClassContext.Tests.AddAsync(dbOrmTestClass);
            await _dbOrmTestClassContext.SaveChangesAsync();
            return Ok();
        }
    }
}