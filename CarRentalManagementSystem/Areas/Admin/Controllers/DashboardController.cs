using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= Dashboard =================
        public IActionResult Index()
        {
            // 🔒 Session + Role check (only Admin allowed)
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                // 🚪 Redirect to global login page
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // 📊 Dashboard Stats
            ViewBag.TotalCustomers = _context.Users.Count(u => u.Role == "Customer");
            ViewBag.TotalCars = _context.Cars.Count();
            ViewBag.TotalBookings = _context.Bookings.Count();

            // 🆕 Last 10 Bookings
            var bookings = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Car)
                .OrderByDescending(b => b.BookingID)
                .Take(10)
                .ToList();

            return View(bookings);
        }

        // 🚪 Admin Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // clear session
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}
