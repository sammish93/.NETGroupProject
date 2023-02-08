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

        [HttpPost]
        public async Task<ActionResult> Create(DbOrmTestClass dbOrmTestClass)
        {
            await _dbOrmTestClassContext.Tests.AddAsync(dbOrmTestClass);
            await _dbOrmTestClassContext.SaveChangesAsync();
            return Ok();
        }

       
    }
}