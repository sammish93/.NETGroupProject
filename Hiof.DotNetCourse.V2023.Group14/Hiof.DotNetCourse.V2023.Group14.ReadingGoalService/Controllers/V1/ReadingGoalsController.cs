using Microsoft.AspNetCore.Mvc;

namespace Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Controllers.V1
{
    public class ReadingGoalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
