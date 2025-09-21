using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Decor_Vista.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
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
        public async Task<IActionResult> FAQ()
        {
            var faqs = await _context.FAQs
                .Where(f => f.IsActive)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View("faq", faqs);
        }                       
        public IActionResult ShopCatalog()
        {
            return RedirectToAction("Catalog", "Product");
        }
        public IActionResult ShopCart()
        {
            return RedirectToAction("Cart", "Product");
        }
        public IActionResult ShopSingle()
        {
            return RedirectToAction("Details", "Product");
        }
        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogDetail()
        {
            return View();
        }


        public IActionResult InspirationGallery()
        {
            return RedirectToAction("Browse", "Gallery");
        }

        
        public IActionResult ProductCatalog()
        {
            return RedirectToAction("Catalog", "Product");
        }

        //public IActionResult Error()
        //{
        //    return View();
        //}

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
