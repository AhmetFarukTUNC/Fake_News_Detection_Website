using Microsoft.AspNetCore.Mvc;
using FakeNewsMVC.Models;

using FakeNewsMVC.Data;
using YourProject.Models;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Contact(ContactMessage model)
    {
        if (ModelState.IsValid)
        {
            // Session'dan UserId al (int? tipinde döner)
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "You must be logged in to send a message.";
                return RedirectToAction("Contact");
            }

            var message = new ContactMessage
            {
                FullName = model.FullName,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                UserId = userId.Value // nullable olduğu için .Value kullan
            };

            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToAction("Contact");
        }

        return View(model);
    }



}
