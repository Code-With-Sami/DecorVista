using System.Diagnostics;
using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;

namespace Decor_Vista.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PortfolioDetail()
        {
            return View();
        }
        public IActionResult PortfolioGrid()
        {
            return View();
        }
        public IActionResult ServiceList()
        {
            return View();
        }
        public IActionResult ServiceDetail()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult ShopCatalog()
        {
            return View();
        }
        public IActionResult ShopCart()
        {
            return View();
        }
        public IActionResult ShopSingle()
        {
            return View();
        }
        public IActionResult Commingsoon()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
