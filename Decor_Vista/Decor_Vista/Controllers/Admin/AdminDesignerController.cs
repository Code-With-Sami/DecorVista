using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Decor_Vista.Models;
namespace Decor_Vista.Controllers.Admin
{
    public class AdminDesignerController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminDesignerController(ApplicationContext context)
        {
            _context = context;
        }

        // List all designers
        public IActionResult Index()
        {
            var designers = _context.Designers.ToList();
            return View(designers);
        }

        // List pending designers
        public IActionResult PendingDesigner()
        {
            var designers = _context.Designers.Where(d => d.Status == "Pending").ToList();
            return View(designers);
        }

        // List approved designers
        public IActionResult ApprovedDesigner()
        {
            var designers = _context.Designers.Where(d => d.Status == "Approved").ToList();
            return View(designers);
        }

        // List rejected designers
        public IActionResult RejectedDesigner()
        {
            var designers = _context.Designers.Where(d => d.Status == "Rejected").ToList();
            return View(designers);
        }

        // Approve designer
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var designer = _context.Designers.FirstOrDefault(d => d.Id == id);
            if (designer != null)
            {
                designer.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("PendingDesigner");
        }

        // Reject designer
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var designer = _context.Designers.FirstOrDefault(d => d.Id == id);
            if (designer != null)
            {
                designer.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("PendingDesigner");
        }
    }
}