using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashedBytes).ToLower();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            ModelState.Remove("Img");
            if (ModelState.IsValid)
            {
                user.password = HashPassword(user.password);
                user.Img = "/Admin/Images/default.png";
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email && u.password == hashedPassword);
            if (user != null)
            {
                TempData["success"] = "Login successful!";
                // Redirect to user dashboard after login
                return RedirectToAction("Index", "UserDashboard");
            }
            TempData["error"] = "Invalid email or password.";
            return View();
        }
    }
}
