using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Decor_Vista.Controllers
{
    public class DesignerPortfolioController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _env;

        public DesignerPortfolioController(ApplicationContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            int designerId = HttpContext.Session.GetInt32("DesignerId") ?? 0;
            var portfolios = _context.Portfolios
                .Include(p => p.Category)
                .Where(p => p.DesignerId == designerId)
                .ToList();
            return View(portfolios);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.GalleryCategories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Portfolio portfolio, IFormFile MainImage, List<IFormFile> AdditionalImages)
        {
            int designerId = HttpContext.Session.GetInt32("DesignerId") ?? 0;
            portfolio.DesignerId = designerId;

            if (MainImage != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                string path = Path.Combine(_env.WebRootPath, "uploads/portfolio", fileName);
                var directoryPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using var stream = new FileStream(path, FileMode.Create);
                MainImage.CopyTo(stream);
                portfolio.MainImagePath = "/uploads/portfolio/" + fileName;
            }

            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            if (AdditionalImages != null)
            {
                foreach (var img in AdditionalImages)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                    string path = Path.Combine(_env.WebRootPath, "uploads/portfolio", fileName);
                    var directoryPath = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    using var stream = new FileStream(path, FileMode.Create);
                    img.CopyTo(stream);

                    _context.PortfolioImages.Add(new PortfolioImage
                    {
                        PortfolioId = portfolio.PortfolioId,
                        ImagePath = "/uploads/portfolio/" + fileName
                    });
                }
                _context.SaveChanges();
            }

            TempData["Message"] = "Portfolio created successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult ManageImages(int id)
        {
            var portfolio = _context.Portfolios
                .Include(p => p.Images)
                .FirstOrDefault(p => p.PortfolioId == id);
            if (portfolio == null) return NotFound();
            return View(portfolio);
        }

        [HttpPost]
        public IActionResult AddImages(int id, List<IFormFile> AdditionalImages)
        {
            foreach (var img in AdditionalImages)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                string path = Path.Combine(_env.WebRootPath, "uploads/portfolio", fileName);
                var directoryPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using var stream = new FileStream(path, FileMode.Create);
                img.CopyTo(stream);

                _context.PortfolioImages.Add(new PortfolioImage
                {
                    PortfolioId = id,
                    ImagePath = "/uploads/portfolio/" + fileName
                });
            }
            _context.SaveChanges();

            TempData["Message"] = "Images added successfully.";
            return RedirectToAction("ManageImages", new { id });
        }

        [HttpPost]
        public IActionResult DeleteImage(int imageId)
        {
            var img = _context.PortfolioImages.Find(imageId);
            if (img != null)
            {
                _context.PortfolioImages.Remove(img);
                _context.SaveChanges();
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var portfolio = _context.Portfolios.Include(p => p.Images)
                .FirstOrDefault(p => p.PortfolioId == id);
            if (portfolio != null)
            {
                _context.PortfolioImages.RemoveRange(portfolio.Images);
                _context.Portfolios.Remove(portfolio);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
