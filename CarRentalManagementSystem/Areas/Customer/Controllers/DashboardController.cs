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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return RedirectToAction("Login", "Account");

            int customerId = HttpContext.Session.GetInt32("CustomerID").GetValueOrDefault();

            var myBookings = _context.Bookings
                                     .Include(b => b.Car)
                                     .Where(b => b.UserID == customerId)
                                     .OrderByDescending(b => b.PickupDate) // ✅ Fix: BookingDate → PickupDate
                                     .ToList();

            return View(myBookings);
        }
    }
}
