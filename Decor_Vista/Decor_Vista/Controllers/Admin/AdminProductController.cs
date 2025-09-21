using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers.Admin
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminProductController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Categories = new SelectList(await _context.ProductCategories.Where(c => c.IsActive).OrderBy(c => c.CategoryName).ToListAsync(), "CategoryName", "CategoryName");
        }

        // GET: AdminProduct (Admin)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.OrderByDescending(p => p.CreatedAt).ToListAsync());
        }

        // GET: AdminProduct/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: AdminProduct/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Category,Brand,ShortDescription,DetailedDescription,Price,SalePrice,Dimensions,Materials,Color,Style,Weight,SKU,StockQuantity,IsAvailable,IsFeatured,PurchaseLink,PartnerWebsite,Tags,Rating,ReviewCount")] Product product, IFormFile? mainImage, List<IFormFile>? additionalImages)
        {
            if (ModelState.IsValid)
            {
                // Handle main image upload
                if (mainImage != null && mainImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + mainImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await mainImage.CopyToAsync(fileStream);
                    }

                    product.MainImagePath = "/uploads/products/" + uniqueFileName;
                }

                // Handle additional images upload
                if (additionalImages != null && additionalImages.Any())
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var additionalImagePaths = new List<string>();

                    foreach (var image in additionalImages)
                    {
                        if (image.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }

                            additionalImagePaths.Add("/uploads/products/" + uniqueFileName);
                        }
                    }

                    product.AdditionalImagesPath = string.Join("|", additionalImagePaths);
                }

                product.CreatedAt = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View("~/Views/Admin/Product/Create.cshtml", product);
        }

        // GET: AdminProduct/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            await PopulateDropdowns();
            return View(product);
        }

        // POST: AdminProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Category,Brand,ShortDescription,DetailedDescription,Price,SalePrice,Dimensions,Materials,Color,Style,Weight,SKU,StockQuantity,IsAvailable,IsFeatured,PurchaseLink,PartnerWebsite,Tags,Rating,ReviewCount,CreatedAt")] Product product, IFormFile? mainImage, List<IFormFile>? additionalImages)
        {
            if (id != product.ProductId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var productFromDb = await _context.Products.FindAsync(id);
                    if (productFromDb == null) return NotFound();

                    // Handle main image upload
                    if (mainImage != null && mainImage.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(productFromDb.MainImagePath))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productFromDb.MainImagePath?.TrimStart('/') ?? "");
                            if (System.IO.File.Exists(oldImagePath)) { System.IO.File.Delete(oldImagePath); }
                        }

                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + mainImage.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await mainImage.CopyToAsync(fileStream);
                        }

                        product.MainImagePath = "/uploads/products/" + uniqueFileName;
                    }
                    else
                    {
                        product.MainImagePath = productFromDb.MainImagePath;
                    }

                    // Handle additional images upload
                    if (additionalImages != null && additionalImages.Any())
                    {
                        // Delete old additional images if exist
                        if (!string.IsNullOrEmpty(productFromDb.AdditionalImagesPath))
                        {
                            var oldImages = productFromDb.AdditionalImagesPath.Split('|');
                            foreach (var oldImage in oldImages)
                            {
                                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldImage?.TrimStart('/') ?? "");
                                if (System.IO.File.Exists(oldImagePath)) { System.IO.File.Delete(oldImagePath); }
                            }
                        }

                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var additionalImagePaths = new List<string>();

                        foreach (var image in additionalImages)
                        {
                            if (image.Length > 0)
                            {
                                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }

                                additionalImagePaths.Add("/uploads/products/" + uniqueFileName);
                            }
                        }

                        product.AdditionalImagesPath = string.Join("|", additionalImagePaths);
                    }
                    else
                    {
                        product.AdditionalImagesPath = productFromDb.AdditionalImagesPath;
                    }

                    product.UpdatedAt = DateTime.Now;
                    _context.Entry(productFromDb).CurrentValues.SetValues(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(product);
        }

        // GET: AdminProduct/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: AdminProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                // Delete main image
                if (!string.IsNullOrEmpty(product.MainImagePath))
                {
                    var mainImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.MainImagePath?.TrimStart('/') ?? "");
                    if (System.IO.File.Exists(mainImagePath)) { System.IO.File.Delete(mainImagePath); }
                }

                // Delete additional images
                if (!string.IsNullOrEmpty(product.AdditionalImagesPath))
                {
                    var additionalImages = product.AdditionalImagesPath.Split('|');
                    foreach (var image in additionalImages)
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, image?.TrimStart('/') ?? "");
                        if (System.IO.File.Exists(imagePath)) { System.IO.File.Delete(imagePath); }
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
