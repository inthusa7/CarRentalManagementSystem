using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var dashboard = new Dashboard
            {
                TotalCustomers = _context.Users.Count(u => u.Role == "Customer"),
                TotalCars = _context.Cars.Count(),
                AvailableCars = _context.Cars.Count(c => c.IsAvailable),
                TotalBookings = _context.Bookings.Count(),
                ActiveBookings = _context.Bookings.Count(b => b.ReturnDate >= DateTime.Now),
                TotalRevenue = _context.Bookings.Sum(b => (decimal?)b.TotalCost) ?? 0
            };

            return View(dashboard);
        }
    }
}
