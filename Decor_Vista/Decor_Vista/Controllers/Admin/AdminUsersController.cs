using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography; 
using System.Text;
using Decor_Vista.ViewModels;

namespace Decor_Vista.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationContext _context;

        public AdminUsersController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashedBytes).ToLower();
            }
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.user_id == id);
            if (user == null) return NotFound();
            return View(user);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, IFormFile? imgFile) 
        {
            ModelState.Remove("Img"); 

            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
            
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadDir = Path.Combine(wwwRootPath, "Admin", "Images", "Users");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                    string imagePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }
                    user.Img = "/Admin/Images/Users/" + fileName;
                }
                else
                {
                 
                    user.Img = "/Admin/Images/default.png"; 
                }

                user.password = HashPassword(user.password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // --- GET: EDIT ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, IFormFile? imgFile)
        {
            if (id != user.user_id) return NotFound();

    
            ModelState.Remove("password");
            ModelState.Remove("Img");

            if (ModelState.IsValid)
            {
                try
                {
                    var userFromDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.user_id == id);

   
                    if (imgFile != null)
                    {
               
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string uploadDir = Path.Combine(wwwRootPath, "Admin", "Images", "Users");
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                        string imagePath = Path.Combine(uploadDir, fileName);
                        using (var fileStream = new FileStream(imagePath, FileMode.Create)) { await imgFile.CopyToAsync(fileStream); }
                        user.Img = "/Admin/Images/Users/" + fileName;
                    }
                    else
                    {
                        user.Img = userFromDb.Img; 
                    }

      
                    user.password = userFromDb.password;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "User details updated successfully!";
                }
                catch (DbUpdateConcurrencyException) { throw; }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user == null)
                {
                    TempData["error"] = "User not found.";
                    return RedirectToAction("Edit", new { id = model.UserId });
                }

 
                var hashedOldPassword = HashPassword(model.OldPassword);
                if (user.password != hashedOldPassword)
                {
                    TempData["error"] = "Incorrect current password.";
                    return RedirectToAction("Edit", new { id = model.UserId });
                }


                user.password = HashPassword(model.NewPassword);
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["success"] = "Password changed successfully!";
                return RedirectToAction("Index");
            }

     
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["error"] = "Password change failed: " + errors;
            return RedirectToAction("Edit", new { id = model.UserId });
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.user_id == id);
            if (user == null) return NotFound();
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
              
                string defaultImagePath = "/Admin/Images/default.png";
                if (!string.IsNullOrEmpty(user.Img) && user.Img != defaultImagePath)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.Img.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "User deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}