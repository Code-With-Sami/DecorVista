using Microsoft.AspNetCore.Mvc;

namespace Decor_Vista.Controllers
{
    public class UserDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
