using Decor_Vista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Decor_Vista.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationContext _context;

        public ContactController(ApplicationContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Submit([Bind("Name,Email,PhoneNumber,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    contact.CreatedAt = DateTime.Now;
                    contact.IsRead = false;

                    _context.Contacts.Add(contact);
                    await _context.SaveChangesAsync();

           
                    TempData["success"] = "Thank you for your message! We will get back to you soon.";
                }
                catch (Exception)
                {
                    TempData["error"] = "Sorry, there was an error sending your message. Please try again later.";
                }
            }
            else
            {
    
                TempData["error"] = "Please ensure all required fields are filled out correctly.";
            }



	
			return View("~/Views/Home/Contact.cshtml");
        }
    }
}