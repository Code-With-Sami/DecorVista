using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers.Admin
{
    public class AdminFAQController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminFAQController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminFAQ
        public async Task<IActionResult> Index()
        {
            return View(await _context.FAQs.OrderByDescending(f => f.CreatedAt).ToListAsync());
        }

        // GET: AdminFAQ/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminFAQ/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                fAQ.CreatedAt = DateTime.Now;
                fAQ.UpdatedAt = DateTime.Now;
                _context.Add(fAQ);
                await _context.SaveChangesAsync();
                TempData["success"] = "FAQ created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(fAQ);
        }

        // GET: AdminFAQ/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ == null) return NotFound();
            return View(fAQ);
        }

        // POST: AdminFAQ/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FAQ fAQ)
        {
            if (id != fAQ.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingFAQ = await _context.FAQs.FindAsync(id);
                    if (existingFAQ == null) return NotFound();

                    existingFAQ.Question = fAQ.Question;
                    existingFAQ.Answer = fAQ.Answer;
                    existingFAQ.IsActive = fAQ.IsActive;
                    existingFAQ.UpdatedAt = DateTime.Now;

                    _context.Update(existingFAQ);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "FAQ updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(fAQ.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fAQ);
        }

        // GET: AdminFAQ/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var fAQ = await _context.FAQs.FirstOrDefaultAsync(m => m.Id == id);
            if (fAQ == null) return NotFound();
            return View(fAQ);
        }

        // POST: AdminFAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ != null)
            {
                _context.FAQs.Remove(fAQ);
                await _context.SaveChangesAsync();
                TempData["success"] = "FAQ deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminFAQ/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var fAQ = await _context.FAQs.FirstOrDefaultAsync(m => m.Id == id);
            if (fAQ == null) return NotFound();
            return View(fAQ);
        }

        private bool FAQExists(int id)
        {
            return _context.FAQs.Any(e => e.Id == id);
        }
    }
}
