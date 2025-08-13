using Microsoft.AspNetCore.Mvc;

namespace Decor_Vista.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = "Shahan"; // optional
            return View("Dashboard"); // Direct Dashboard.cshtml load karega
        }

        public IActionResult Dashboard()
        {
            ViewBag.UserName = "Shahan";
            return View();
        }
    }
}
