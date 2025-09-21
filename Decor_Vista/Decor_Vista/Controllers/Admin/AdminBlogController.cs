using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers.Admin
{
    public class AdminBlogController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminBlogController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: AdminBlog
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.OrderByDescending(b => b.CreatedAt).ToListAsync());
        }

        // GET: AdminBlog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminBlog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile? imgFile)
        {
            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    if (imgFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImagePath", "File size cannot exceed 5MB.");
                        return View(blog);
                    }

                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Admin", "Images", "Blog");
                    if (!Directory.Exists(uploadDir)) { Directory.CreateDirectory(uploadDir); }

                    string fileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fs);
                    }
                    blog.ImagePath = "/Admin/Images/Blog/" + fileName;
                }

                blog.CreatedAt = DateTime.Now;
                blog.UpdatedAt = DateTime.Now;
                
                if (blog.IsPublished)
                {
                    blog.PublishedDate = DateTime.Now;
                }

                _context.Add(blog);
                await _context.SaveChangesAsync();
                TempData["success"] = "Blog post created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: AdminBlog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        // POST: AdminBlog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog, IFormFile? imgFile)
        {
            if (id != blog.Id) return NotFound();
            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBlog = await _context.Blogs.FindAsync(id);
                    if (existingBlog == null) return NotFound();

                    if (imgFile != null)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingBlog.ImagePath))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingBlog.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath)) { System.IO.File.Delete(oldImagePath); }
                        }

                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Admin", "Images", "Blog");
                        string fileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                        string filePath = Path.Combine(uploadDir, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(fileStream);
                        }
                        existingBlog.ImagePath = "/Admin/Images/Blog/" + fileName;
                    }

                    existingBlog.Title = blog.Title;
                    existingBlog.Content = blog.Content;
                    existingBlog.ShortDescription = blog.ShortDescription;
                    existingBlog.Author = blog.Author;
                    existingBlog.Tags = blog.Tags;
                    existingBlog.Category = blog.Category;
                    existingBlog.IsPublished = blog.IsPublished;
                    existingBlog.UpdatedAt = DateTime.Now;

                    if (blog.IsPublished && !existingBlog.IsPublished)
                    {
                        existingBlog.PublishedDate = DateTime.Now;
                    }

                    _context.Update(existingBlog);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Blog post updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: AdminBlog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        // POST: AdminBlog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                // Delete image if exists
                if (!string.IsNullOrEmpty(blog.ImagePath))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, blog.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath)) { System.IO.File.Delete(imagePath); }
                }

                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
                TempData["success"] = "Blog post deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminBlog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
