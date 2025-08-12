using Decor_Vista.Models;
using Decor_Vista.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Decor_Vista.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationContext _context;
        public AuthController(ApplicationContext context) => _context = context;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.FullName ?? "");
                    return RedirectToAction("Dashboard", "User");
                }

                ModelState.AddModelError("", "Invalid email or password");
            }

            return View(model);
        }
   


        // GET: Register Page
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user, string ConfirmPassword)
        {
            if (user.PasswordHash != ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View(user);
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                // ✅ Registration ke baad Login page pe bhej do
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Error = "Please fill all fields correctly.";
            return View(user);
        }


        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
