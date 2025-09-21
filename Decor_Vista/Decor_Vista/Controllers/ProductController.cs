using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

       public async Task<IActionResult> Catalog(string? category, string? search, decimal? minPrice, decimal? maxPrice, string? sortBy)
        {
            var query = _context.Products.Where(p => p.IsAvailable).AsQueryable();

     
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.ShortDescription.Contains(search));
            }

  
            if (minPrice.HasValue)
            {
                query = query.Where(p => (p.SalePrice ?? p.Price) >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => (p.SalePrice ?? p.Price) <= maxPrice.Value);
            }


            switch (sortBy?.ToLower())
            {
                case "price_low":
                    query = query.OrderBy(p => (p.SalePrice ?? p.Price));
                    break;
                case "price_high":
                    query = query.OrderByDescending(p => (p.SalePrice ?? p.Price));
                    break;
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                default:
                    query = query.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.CreatedAt);
                    break;
            }

            var products = await query.ToListAsync();


            ViewBag.Categories = await _context.ProductCategories
                                              .Where(c => c.IsActive)
                                              .Select(c => c.CategoryName)
                                              .Distinct()
                                              .ToListAsync();
            ViewBag.Brands = await _context.Products
                                        .Where(p => !string.IsNullOrEmpty(p.Brand))
                                        .Select(p => p.Brand)
                                        .Distinct()
                                        .OrderBy(b => b)
                                        .ToListAsync();

            ViewBag.Styles = await _context.Products.Where(p => !string.IsNullOrEmpty(p.Style)).Select(p => p.Style).Distinct().OrderBy(s => s).ToListAsync();



            ViewBag.CurrentCategory = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;

            return View(products);
        }
        // GET: Product/Details/5 (User-facing)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            // Get cart items if user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var cartItem = await _context.Cart
                    .FirstOrDefaultAsync(c => c.UserId == userId.Value && c.ProductId == id);
                ViewBag.CartQuantity = cartItem?.Quantity ?? 0;
            }

            return View(product);
        }

        // POST: Product/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Please log in to add items to cart." });
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            if (quantity > product.StockQuantity)
            {
                return Json(new { success = false, message = "Requested quantity exceeds available stock." });
            }

            var existingCartItem = await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId.Value && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.UpdatedAt = DateTime.Now;
            }
            else
            {
                var cartItem = new Cart
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.Now
                };
                _context.Cart.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product added to cart successfully!" });
        }

        // POST: Product/UpdateCartQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateCartQuantity(int productId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Please log in to update cart." });
            }

            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId.Value && c.ProductId == productId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found." });
            }

            if (quantity <= 0)
            {
                _context.Cart.Remove(cartItem);
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null && quantity > product.StockQuantity)
                {
                    return Json(new { success = false, message = "Requested quantity exceeds available stock." });
                }

                cartItem.Quantity = quantity;
                cartItem.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cart updated successfully!" });
        }

        // POST: Product/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Please log in to remove items from cart." });
            }

            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId.Value && c.ProductId == productId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found." });
            }

            _context.Cart.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Item removed from cart successfully!" });
        }

        // GET: Product/Cart
        public async Task<IActionResult> Cart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await _context.Cart
                .Include(c => c.Product)
                .Where(c => c.UserId == userId.Value)
                .ToListAsync();

            return View(cartItems);
        }
    }
}
