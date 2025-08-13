using Decor_Vista.Models;
using DecorVista.Models;
using Microsoft.AspNetCore.Mvc;

namespace Decor_Vista.Controllers.Designer
{
    public class DesignerController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DesignerController(ApplicationContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null)
            {
                return RedirectToAction("LoginDesigner");
            }
 
            return View();
        }

        [HttpGet]
        public IActionResult RegisterDesigner()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterDesigner(DecorVista.Models.Designer model, IFormFile ProfileImage, List<IFormFile> PortfolioSamples)
        {
            if (ModelState.IsValid)
            {
                // Set status to pending
                model.Status = "Pending";

                // Handle profile image
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);
                    var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/designers", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(uploadPath) ?? string.Empty);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(stream);
                    }

                    model.ProfileImagePath = "/uploads/designers/" + fileName;
                }

                // Handle portfolio files (upload only, no DB storage)
                if (PortfolioSamples != null && PortfolioSamples.Any())
                {
                    foreach (var file in PortfolioSamples)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/portfolio", fileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(uploadPath) ?? string.Empty);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                _db.Designers.Add(model);
                _db.SaveChanges();

                TempData["Message"] = "Registration submitted. Please wait for admin approval.";
                return RedirectToAction("PendingApproval");
            }

            return View(model);

        }


        [HttpGet]
        public IActionResult LoginDesigner()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginDesigner(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Please enter email and password.";
                return View();
            }

            var designer = _db.Designers
                .FirstOrDefault(d => d.Email == email && d.Password == password);

            if (designer == null)
            {
                TempData["Error"] = "Invalid email or password.";
                return View();
            }

            if (designer.Status != "Approved")
            {
                TempData["Error"] = "Your account is not approved yet. Please wait for admin approval.";
                return View();
            }

            HttpContext.Session.SetInt32("DesignerId", designer.Id);
            HttpContext.Session.SetString("DesignerName", designer.FirstName + " " + designer.LastName);

            return RedirectToAction("Index");
        }

    }
}
