using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers
{
	public class GalleryController : Controller
	{
		private readonly ApplicationContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public GalleryController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: Gallery/Browse (User-facing)
		public async Task<IActionResult> Browse(string? roomType, string? theme, string? colorScheme)
		{
			var query = _context.Gallery.AsQueryable();

			if (!string.IsNullOrEmpty(roomType))
				query = query.Where(g => g.Category == roomType);
			
			if (!string.IsNullOrEmpty(theme))
				query = query.Where(g => g.Theme == theme);
			
			if (!string.IsNullOrEmpty(colorScheme))
				query = query.Where(g => g.ColorScheme == colorScheme);

			var galleries = await query.OrderByDescending(g => g.CreatedAt).ToListAsync();

			// Get filter options for the view
			ViewBag.RoomTypes = await _context.GalleryCategories.OrderBy(c => c.CategoryName).ToListAsync();
			ViewBag.Themes = await _context.GalleryThemes.OrderBy(t => t.ThemeName).ToListAsync();
			ViewBag.ColorSchemes = await _context.GalleryColorSchemes.OrderBy(c => c.ColorSchemeName).ToListAsync();

			// Get user favorites if user is logged in
			var userId = HttpContext.Session.GetInt32("UserId");
			if (userId.HasValue)
			{
				var userFavorites = await _context.UserFavorites
					.Where(uf => uf.UserId == userId.Value)
					.Select(uf => uf.GalleryId)
					.ToListAsync();
				ViewBag.UserFavorites = userFavorites;
			}

			return View(galleries);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var galleryItem = await _context.Gallery
				.FirstOrDefaultAsync(m => m.GalleryId == id);

			if (galleryItem == null)
			{
				return NotFound();
			}

			return View(galleryItem);
		}

		// POST: Gallery/ToggleFavorite
		[HttpPost]
		public async Task<IActionResult> ToggleFavorite(int galleryId)
		{
			var userId = HttpContext.Session.GetInt32("UserId");
			if (!userId.HasValue)
			{
				return Json(new { success = false, message = "Please login to save favorites." });
			}

			var existingFavorite = await _context.UserFavorites
				.FirstOrDefaultAsync(uf => uf.UserId == userId.Value && uf.GalleryId == galleryId);

			if (existingFavorite != null)
			{
				_context.UserFavorites.Remove(existingFavorite);
				await _context.SaveChangesAsync();
				return Json(new { success = true, isFavorite = false, message = "Removed from favorites." });
			}
			else
			{
				var newFavorite = new UserFavorites
				{
					UserId = userId.Value,
					GalleryId = galleryId,
					CreatedAt = DateTime.Now
				};
				_context.UserFavorites.Add(newFavorite);
				await _context.SaveChangesAsync();
				return Json(new { success = true, isFavorite = true, message = "Added to favorites." });
			}
		}

		// GET: Gallery/MyFavorites
		public async Task<IActionResult> MyFavorites()
		{
			var userId = HttpContext.Session.GetInt32("UserId");
			if (!userId.HasValue)
			{
				return RedirectToAction("Login", "Auth");
			}

			var favorites = await _context.UserFavorites
				.Include(uf => uf.Gallery)
				.Where(uf => uf.UserId == userId.Value)
				.OrderByDescending(uf => uf.CreatedAt)
				.ToListAsync();

			return View(favorites);
		}
	}
}