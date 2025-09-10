using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
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
        public IActionResult Index()
        {
            // Session check: if admin not logged in, redirect to login
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUsername")))
                return RedirectToAction("Login", "Account");

            // Dashboard data
            ViewBag.TotalCustomers = _context.Users.Count(u => u.Role == "Customer");
            ViewBag.TotalCars = _context.Cars.Count();
            ViewBag.AvailableCars = _context.Cars.Count(c => c.IsAvailable);
            ViewBag.RentedCars = _context.Cars.Count(c => !c.IsAvailable);
            ViewBag.TotalBookings = _context.Bookings.Count();
            ViewBag.TotalRevenue = _context.Bookings.Sum(b => b.TotalCost);


            var recentBookings = _context.Bookings
                                         .Include(b => b.User)
                                         .Include(b => b.Car)
                                         .OrderByDescending(b => b.PickupDate)
                                         .Take(5)
                                         .ToList();

            return View(recentBookings);
            
        }
    }
}
