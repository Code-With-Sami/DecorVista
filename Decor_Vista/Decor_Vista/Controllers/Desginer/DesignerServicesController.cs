using Decor_Vista.Controllers.Desginer;
using Decor_Vista.Models;
using DecorVista.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DecorVista.Controllers
{
    public class DesignerServicesController : BaseDesignerController
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _env;

        public DesignerServicesController(ApplicationContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Services
        public IActionResult Index()
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            var services = _db.DesignerServices
                              .Where(s => s.DesignerId == designerId)
                              .OrderByDescending(s => s.CreatedAt)
                              .ToList();

            return View(services);
        }

        // GET: /Services/Create
        [HttpGet]
        public IActionResult Create()
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            return View(new DesignerService
            {
                PricingType = PricingType.StartingFrom,
                IsActive = true
            });
        }

        // POST: /Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DesignerService model, IFormFile? CoverImage)
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            // Server-side validation for price
            if ((model.PricingType == PricingType.Hourly || model.PricingType == PricingType.Fixed) && (!model.Price.HasValue || model.Price.Value <= 0))
            {
                ModelState.AddModelError(nameof(model.Price), "Price is required and must be greater than 0 for Hourly or Fixed pricing.");
            }

            if (!ModelState.IsValid)
                return View(model);

            model.DesignerId = designerId.Value;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            // Handle cover image
            if (CoverImage != null && CoverImage.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(CoverImage.FileName);
                var folder = Path.Combine(_env.WebRootPath, "uploads/services");
                Directory.CreateDirectory(folder);
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    CoverImage.CopyTo(stream);
                }
                model.CoverImagePath = "/uploads/services/" + fileName;
            }

            _db.DesignerServices.Add(model);
            _db.SaveChanges();

            TempData["Message"] = "Service created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Services/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            var service = _db.DesignerServices.FirstOrDefault(s => s.Id == id && s.DesignerId == designerId.Value);
            if (service == null) return NotFound();

            return View(service);
        }

        // POST: /Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DesignerService model, IFormFile? CoverImage, bool removeImage = false)
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            var service = _db.DesignerServices.FirstOrDefault(s => s.Id == id && s.DesignerId == designerId.Value);
            if (service == null) return NotFound();

            // Server-side validation for price
            if ((model.PricingType == PricingType.Hourly || model.PricingType == PricingType.Fixed) && (!model.Price.HasValue || model.Price.Value <= 0))
            {
                ModelState.AddModelError(nameof(model.Price), "Price is required and must be greater than 0 for Hourly or Fixed pricing.");
            }

            if (!ModelState.IsValid)
                return View(service); // return the existing service to keep values

            // Update allowed fields
            service.Title = model.Title;
            service.Category = model.Category;
            service.Description = model.Description;
            service.PricingType = model.PricingType;
            service.Price = model.Price;
            service.DurationText = model.DurationText;
            service.IsActive = model.IsActive;
            service.IsFeatured = model.IsFeatured;
            service.UpdatedAt = DateTime.UtcNow;

            // Cover image logic
            if (removeImage && !string.IsNullOrEmpty(service.CoverImagePath))
            {
                // Optionally delete physical file
                TryDeleteFile(service.CoverImagePath);
                service.CoverImagePath = null;
            }

            if (CoverImage != null && CoverImage.Length > 0)
            {
                // replace existing
                if (!string.IsNullOrEmpty(service.CoverImagePath))
                    TryDeleteFile(service.CoverImagePath);

                var fileName = Guid.NewGuid() + Path.GetExtension(CoverImage.FileName);
                var folder = Path.Combine(_env.WebRootPath, "uploads/services");
                Directory.CreateDirectory(folder);
                var fullPath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    CoverImage.CopyTo(stream);
                }
                service.CoverImagePath = "/uploads/services/" + fileName;
            }

            _db.SaveChanges();
            TempData["Message"] = "Service updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Services/Toggle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id)
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            var service = _db.DesignerServices.FirstOrDefault(s => s.Id == id && s.DesignerId == designerId.Value);
            if (service == null) return NotFound();

            service.IsActive = !service.IsActive;
            service.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();

            TempData["Message"] = $"Service {(service.IsActive ? "activated" : "deactivated")} successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Services/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var designerId = HttpContext.Session.GetInt32("DesignerId");
            if (designerId == null) return RedirectToAction("LoginDesigner", "Designer");

            var service = _db.DesignerServices.FirstOrDefault(s => s.Id == id && s.DesignerId == designerId.Value);
            if (service == null) return NotFound();

            if (!string.IsNullOrEmpty(service.CoverImagePath))
                TryDeleteFile(service.CoverImagePath);

            _db.DesignerServices.Remove(service);
            _db.SaveChanges();

            TempData["Message"] = "Service deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private void TryDeleteFile(string webPath)
        {
            try
            {
                var full = Path.Combine(_env.WebRootPath, webPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(full))
                    System.IO.File.Delete(full);
            }
            catch { /* swallow */ }
        }


    }
}
