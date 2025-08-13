using Decor_Vista.Controllers.Desginer;
using Decor_Vista.Models;
using DecorVista.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Decor_Vista.Controllers.Designer
{
    public class DesignerController : BaseDesignerController
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
            HttpContext.Session.SetString("DesignerImg", designer.ProfileImagePath);

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("DesignerId") != null)
            {
                HttpContext.Session.Remove("DesignerId");
                return RedirectToAction("LoginDesigner");
            }

            return View();
        }

        private int CalculateProfileCompletion(DecorVista.Models.Designer d)
        {
            int totalFields = 25;
            int filledFields = 0;

            // Helper function for strings
            bool Filled(string? value) => !string.IsNullOrWhiteSpace(value);
            // Helper function for numbers
            bool FilledInt(int? value) => value.HasValue && value.Value > 0;
            bool FilledDecimal(decimal? value) => value.HasValue && value.Value > 0;

            // Personal Info
            if (Filled(d.FirstName)) filledFields++;
            if (Filled(d.LastName)) filledFields++;
            if (Filled(d.PhoneNumber)) filledFields++;
            if (Filled(d.ProfileImagePath)) filledFields++;
            if (Filled(d.Address)) filledFields++;
            if (Filled(d.City)) filledFields++;
            if (Filled(d.Country)) filledFields++;
            if (Filled(d.PostalCode)) filledFields++;

            // Professional Info
            if (Filled(d.Specialization)) filledFields++;
            if (FilledInt(d.YearsOfExperience)) filledFields++;
            if (Filled(d.Skills)) filledFields++;
            if (Filled(d.PortfolioLink)) filledFields++;
            if (Filled(d.WorkCategories)) filledFields++;
            if (FilledDecimal(d.ConsultationFee)) filledFields++;
            if (Filled(d.Availability)) filledFields++;
            if (Filled(d.Bio)) filledFields++;
            if (Filled(d.DesignPhilosophy)) filledFields++;
            if (Filled(d.LanguagesSpoken)) filledFields++;
            if (Filled(d.Certifications)) filledFields++;
            if (Filled(d.PreferredProjectTypes)) filledFields++;
            if (FilledInt(d.MaxProjectsAtOnce)) filledFields++;

            // Social Links
            if (Filled(d.FacebookLink)) filledFields++;
            if (Filled(d.InstagramLink)) filledFields++;
            if (Filled(d.LinkedInLink)) filledFields++;
            if (Filled(d.PinterestLink)) filledFields++;

            return (int)Math.Round(((double)filledFields / totalFields) * 100);
        }


        [HttpGet]
        public IActionResult Profile()
        {
            int? designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner");

            var designer = _db.Designers.FirstOrDefault(d => d.Id == designerId);
            if (designer == null) return RedirectToAction("LoginDesigner");

            ViewBag.ProfileCompletion = CalculateProfileCompletion(designer);
            return View(designer);
        }


        [HttpPost]
        public IActionResult Profile(DecorVista.Models.Designer model, IFormFile? ProfileImage)
        {
            int? designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner");

            var designer = _db.Designers.FirstOrDefault(d => d.Id == designerId);
            if (designer == null) return RedirectToAction("LoginDesigner");

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);
                var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/designers", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(uploadPath) ?? string.Empty);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    ProfileImage.CopyTo(stream);
                }
                designer.ProfileImagePath = "/uploads/designers/" + fileName;
            }

            // Update fields except Email & Password
            designer.FirstName = model.FirstName;
            designer.LastName = model.LastName;
            designer.PhoneNumber = model.PhoneNumber;
            designer.Specialization = model.Specialization;
            designer.YearsOfExperience = model.YearsOfExperience;
            designer.Skills = model.Skills;
            designer.PortfolioLink = model.PortfolioLink;
            designer.WorkCategories = model.WorkCategories;
            designer.ConsultationFee = model.ConsultationFee;
            designer.Availability = model.Availability;
            designer.Address = model.Address;
            designer.City = model.City ?? designer.City;
            designer.Country = model.Country ?? designer.Country;
            designer.PostalCode = model.PostalCode ?? designer.PostalCode;
            designer.Bio = model.Bio;
            designer.DesignPhilosophy = model.DesignPhilosophy;
            designer.LanguagesSpoken = model.LanguagesSpoken;
            designer.FacebookLink = model.FacebookLink;
            designer.InstagramLink = model.InstagramLink;
            designer.LinkedInLink = model.LinkedInLink;
            designer.PinterestLink = model.PinterestLink;
            designer.Certifications = model.Certifications;
            designer.PreferredProjectTypes = model.PreferredProjectTypes;
            designer.MaxProjectsAtOnce = model.MaxProjectsAtOnce;

            _db.SaveChanges();
            TempData["Message"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword)
        {
            int? designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner");

            var designer = _db.Designers.FirstOrDefault(d => d.Id == designerId);
            if (designer == null) return RedirectToAction("LoginDesigner");

            if (designer.Password != currentPassword)
            {
                TempData["Error"] = "Current password is incorrect.";
                return RedirectToAction("Profile");
            }

            designer.Password = newPassword;
            _db.SaveChanges();
            TempData["Message"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }

    }
}
