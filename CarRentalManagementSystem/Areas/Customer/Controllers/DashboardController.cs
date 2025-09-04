using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Session check: if customer not logged in, redirect to login
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account");

            int userId = HttpContext.Session.GetInt32("UserID").GetValueOrDefault();

            // Show customer's own bookings
            var myBookings = _context.Bookings
                                     .Include(b => b.Car)
                                     .Where(b => b.UserID == userId)
                                     .OrderByDescending(b => b.PickupDate)
                                     .ToList();
            return View(myBookings);
        }
    }
}
