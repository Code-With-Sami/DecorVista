using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Decor_Vista.ViewModels;

namespace Decor_Vista.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                return Convert.ToHexString(hashedBytes).ToLower();
            }
        }


        public IActionResult Login()
        {
            return View();
        }


        public IActionResult TestHash()
        {
            var testPassword = "123456";
            var hash = HashPassword(testPassword);
            var expectedHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";
            
            ViewBag.TestPassword = testPassword;
            ViewBag.GeneratedHash = hash;
            ViewBag.ExpectedHash = expectedHash;
            ViewBag.HashMatch = hash == expectedHash;
            ViewBag.AdminCount = _context.Tbl_Admin.Count();
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            try
            {
                var hashedPassword = HashPassword(password);
                var admin = _context.Tbl_Admin.FirstOrDefault(a => 
                    a.Email.ToLower() == email.ToLower() && 
                    a.Password == hashedPassword);

                if (admin != null)
                {
                    HttpContext.Session.SetInt32("AdminId", admin.Id);
                    HttpContext.Session.SetString("AdminName", admin.Name ?? string.Empty);
                    HttpContext.Session.SetString("AdminImg", admin.Img ?? "/Admin/Images/default.png");

                
                    TempData["success"] = "Login successful!";
                    return RedirectToAction(nameof(Dashboard));
                }
                else
                {
          
                    var storedAdmin = _context.Tbl_Admin.FirstOrDefault(a => a.Email.ToLower() == email.ToLower());
                    if (storedAdmin != null)
                    {
                        ViewBag.Error = "Invalid password. Please check your password.";
                        ViewBag.DebugInfo = $"Input Hash: {hashedPassword}, Stored Hash: {storedAdmin.Password}";
                    }
                    else
                    {
                        ViewBag.Error = "Email not found. Please check your email address.";
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Login error: {ex.Message}";
                return View();
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var admins = _context.Tbl_Admin.ToList();
            return View(admins);
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null)
            {
                return RedirectToAction(nameof(Login));
            }
            ViewBag.AdminCount = _context.Tbl_Admin.Count();
            return View();
        }

  
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null) return RedirectToAction(nameof(Login));
            if (id == null) return NotFound();

            var admin = _context.Tbl_Admin.FirstOrDefault(m => m.Id == id);
            if (admin == null) return NotFound();

            return View(admin);
        }


        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("AdminId") == null) return RedirectToAction(nameof(Login));
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admin admin, IFormFile? imgFile)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null) return RedirectToAction(nameof(Login));
            ModelState.Remove("Img");

            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadDir = Path.Combine(wwwRootPath, "Admin", "Images");
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

                    admin.Img = "/Admin/Images/" + fileName;
                }
                else
                {

                    admin.Img = "/Admin/Images/default.png";
                }

                admin.Password = HashPassword(admin.Password);
                _context.Add(admin);
                await _context.SaveChangesAsync();
                TempData["success"] = "Admin registered successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

    

        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null) return RedirectToAction(nameof(Login));
            if (id == null) return NotFound();

            var admin = _context.Tbl_Admin.FirstOrDefault(m => m.Id == id);
            if (admin == null) return NotFound();

            return View(admin);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("AdminId") == null) return RedirectToAction(nameof(Login));

            var admin = await _context.Tbl_Admin.FindAsync(id);
            if (admin != null)
            {
                string defaultImagePath = "/Admin/Images/default.png";

                if (!string.IsNullOrEmpty(admin.Img) && admin.Img != defaultImagePath)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, admin.Img.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.Tbl_Admin.Remove(admin);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Admin deleted successfully!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Profile()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var admin = await _context.Tbl_Admin.FindAsync(adminId.Value);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin); 
        }


        public async Task<IActionResult> EditProfile()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var admin = await _context.Tbl_Admin.FindAsync(adminId.Value);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Admin admin, IFormFile? imgFile)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null || adminId.Value != admin.Id)
            {
                return RedirectToAction(nameof(Login));
            }

            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                var existingAdmin = await _context.Tbl_Admin.FindAsync(adminId.Value);
                if (existingAdmin == null) return NotFound();


                if (imgFile != null)
                {
                    string defaultImagePath = "/Admin/Images/default.png";
                    if (!string.IsNullOrEmpty(existingAdmin.Img) && existingAdmin.Img != defaultImagePath)
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingAdmin.Img.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string uploadDir = Path.Combine(wwwRootPath, "Admin", "Images");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                    string imagePath = Path.Combine(uploadDir, fileName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }
                    existingAdmin.Img = "/Admin/Images/" + fileName;
                }

                existingAdmin.Name = admin.Name;
                existingAdmin.Email = admin.Email;
                _context.Update(existingAdmin);
                await _context.SaveChangesAsync();


                HttpContext.Session.SetString("AdminName", existingAdmin.Name);
                HttpContext.Session.SetString("AdminImg", existingAdmin.Img);

                TempData["success"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile)); 
            }


            return View(admin);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordViewModel model)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var admin = await _context.Tbl_Admin.FindAsync(adminId.Value);
            if (admin == null) return NotFound();

            if (ModelState.IsValid)
            {
                var hashedOldPassword = HashPassword(model.OldPassword);
                if (admin.Password != hashedOldPassword)
                {
                    TempData["error"] = "Incorrect current password.";
                    return RedirectToAction(nameof(EditProfile)); 
                }

                admin.Password = HashPassword(model.NewPassword);
                _context.Update(admin);
                await _context.SaveChangesAsync();

                TempData["success"] = "Password changed successfully!";
                return RedirectToAction(nameof(Profile)); 
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["error"] = "Password change failed: " + string.Join(" ", errors);
            return RedirectToAction(nameof(EditProfile)); 
        }
    }
}