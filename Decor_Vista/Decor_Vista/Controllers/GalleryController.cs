
using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GalleryController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task PopulateCategoriesDropdown()
        {
            ViewBag.Categories = new SelectList(await _context.GalleryCategories.OrderBy(c => c.CategoryName).ToListAsync(), "CategoryName", "CategoryName");
        }

        // GET: Gallery
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gallery.OrderByDescending(g => g.CreatedAt).ToListAsync());
        }

        // GET: Gallery/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropdown();
            return View();
        }

        // POST: Gallery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gallery gallery, IFormFile? imgFile)
        {
            // Clear any automatic validation attached to ImagePath since we set it server-side
            ModelState.Remove("ImagePath");

            if (imgFile == null)
            {
                ModelState.AddModelError("ImagePath", "Please select an image file to upload.");
                await PopulateCategoriesDropdown();
                return View(gallery);
            }

            if (ModelState.IsValid)
            {
                if (imgFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImagePath", "File size cannot exceed 5MB.");
                    await PopulateCategoriesDropdown();
                    return View(gallery);
                }

                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Admin", "Images", "Gallery");
                if (!Directory.Exists(uploadDir)) { Directory.CreateDirectory(uploadDir); }

                string fileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await imgFile.CopyToAsync(fs);
                }
                gallery.ImagePath = "/Admin/Images/Gallery/" + fileName;

                _context.Add(gallery);
                await _context.SaveChangesAsync();
                TempData["success"] = "Gallery item created successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateCategoriesDropdown();
            return View(gallery);
        }

    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var galleryItem = await _context.Gallery.FindAsync(id);
            if (galleryItem == null) return NotFound();
            await PopulateCategoriesDropdown();
            return View(galleryItem);
        }

        // POST: Gallery/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gallery gallery, IFormFile? imgFile)
        {
            if (id != gallery.GalleryId) return NotFound();
            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                var itemFromDb = await _context.Gallery.AsNoTracking().FirstOrDefaultAsync(g => g.GalleryId == id);
                if (itemFromDb == null) return NotFound();

                if (imgFile != null)
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, itemFromDb.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath)) { System.IO.File.Delete(oldImagePath); }

                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Admin", "Images", "Gallery");
                    string fileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    // --- THIS IS THE FIX ---
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream); // Correctly copy the file's stream
                    }
                    gallery.ImagePath = "/Admin/Images/Gallery/" + fileName;
                }
                else
                {
                    gallery.ImagePath = itemFromDb.ImagePath;
                }

                gallery.UpdatedAt = DateTime.Now;
                gallery.CreatedAt = itemFromDb.CreatedAt;
                _context.Update(gallery);
                await _context.SaveChangesAsync();
                TempData["success"] = "Gallery item updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateCategoriesDropdown();
            return View(gallery);
        }

        // GET: Gallery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var galleryItem = await _context.Gallery.FirstOrDefaultAsync(m => m.GalleryId == id);
            if (galleryItem == null) return NotFound();
            return View(galleryItem);
        }

        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryItem = await _context.Gallery.FindAsync(id);
            if (galleryItem != null)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, galleryItem.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath)) { System.IO.File.Delete(imagePath); }
                _context.Gallery.Remove(galleryItem);
                await _context.SaveChangesAsync();
                TempData["success"] = "Gallery item deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galleryItem = await _context.Gallery
                .FirstOrDefaultAsync(m => m.GalleryId == id);

            if (galleryItem == null)
            {
                return NotFound();
            }

            return View(galleryItem);
        }
    }
}