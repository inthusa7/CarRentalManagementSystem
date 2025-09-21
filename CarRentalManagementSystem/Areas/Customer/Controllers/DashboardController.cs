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

        public async Task<IActionResult> Index()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var bookings = await _context.Bookings
                .Include(b => b.Car)
                .Where(b => b.UserID == customerId.Value)
                .OrderByDescending(b => b.PickupDate)
                .ToListAsync();

            ViewBag.TotalBookings = bookings.Count;

            // Only Paid bookings are considered confirmed for Upcoming / Completed stats
            ViewBag.UpcomingBookings = bookings.Count(b => b.IsPaid && b.PickupDate >= DateTime.Today);
            ViewBag.CompletedBookings = bookings.Count(b => b.IsPaid && b.ReturnDate < DateTime.Today);

            return View(bookings);
        }
    }
}
