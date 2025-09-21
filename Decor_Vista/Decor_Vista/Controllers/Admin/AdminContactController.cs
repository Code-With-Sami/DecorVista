using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers.Admin
{
    public class AdminContactController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminContactController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: AdminContact
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.OrderByDescending(c => c.CreatedAt).ToListAsync());
        }

        // GET: AdminContact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null) return NotFound();

            // Mark as read when viewing
            if (!contact.IsRead)
            {
                contact.IsRead = true;
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }

            return View(contact);
        }

        // POST: AdminContact/MarkAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.IsRead = !contact.IsRead;
                _context.Update(contact);
                await _context.SaveChangesAsync();
                TempData["success"] = contact.IsRead ? "Marked as read." : "Marked as unread.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminContact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null) return NotFound();
            return View(contact);
        }

        // POST: AdminContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                TempData["success"] = "Contact message deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminContact/Unread
        public async Task<IActionResult> Unread()
        {
            var unread = await _context.Contacts
                .Where(c => !c.IsRead)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            // Reuse Index view to display the filtered list
            return View("Index", unread);
        }
    }
}
