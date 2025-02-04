using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
