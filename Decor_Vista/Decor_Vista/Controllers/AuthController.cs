//using Decor_Vista.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace Decor_Vista.Controllers
//{
//    public class AuthController : Controller
//    {
//        private readonly ApplicationContext _context;
//        public AuthController(ApplicationContext context) => _context = context;

//        public IActionResult Login() => View();

//        // Login

//        [HttpPost]
//        public IActionResult Login(string uemail, string upassword)
//        {
//            var user = _context.Users.FirstOrDefault(u => u.Email == uemail && u.PasswordHash == upassword );
//            if (user != null)
//            {
//                HttpContext.Session.SetInt32("UserId", user.UserId);
//                HttpContext.Session.SetString("Role", user.Role.ToString());
//                HttpContext.Session.SetString("FullName", user.FullName);

//                return user.Role switch
//                {
//                    RoleEnum.Admin => RedirectToAction("Dashboard", "Admin"),
//                    RoleEnum.EndUser => RedirectToAction("Dashboard", "User"),
//                    RoleEnum.Interiordesigner => RedirectToAction("Dashboard", "Interiordesigner"),
//                    _ => RedirectToAction("Login")
//                };
//            }
//            ViewBag.Error = "Invalid credentials or not approved.";
//            return View();
//        }
//        // Register
//        public IActionResult Register() => View();

//        [HttpPost]
//        public IActionResult Register(User model, RoleEnum requestedRole, string reason)
//        {
//            model.Role = requestedRole;
//            model.DateCreated = DateTime.Now;

//            _context.Users.Add(model);
//            _context.SaveChanges();

//            // Auto-login if active
//            HttpContext.Session.SetInt32("UserId", model.UserId);
//            HttpContext.Session.SetString("Role", model.Role.ToString());

//            return model.Role switch
//            {
//                RoleEnum.Admin => RedirectToAction("Dashboard", "Admin"),
//                RoleEnum.EndUser => RedirectToAction("Dashboard", "User"),
//                RoleEnum.Interiordesigner => RedirectToAction("Dashboard", "Interiordesigner"),
//                _ => RedirectToAction("Login")
//            };
//        }


//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear();
//            return RedirectToAction("Login");
//        }
//    }
//}
