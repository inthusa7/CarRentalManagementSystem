using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var user = await _context.Users.FindAsync(customerId.Value);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User model)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null || customerId != model.UserID)
                return RedirectToAction("Login", "Account", new { area = "" });

            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FindAsync(model.UserID);
            if (user == null)
                return NotFound();

            // Only update editable fields
            user.Email = model.Email;
            user.ContactNo = model.ContactNo;
            user.AltContactNo = model.AltContactNo;
            user.Address = model.Address;
            user.DriverLicenseNo = model.DriverLicenseNo;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index");
        }
    }
}
