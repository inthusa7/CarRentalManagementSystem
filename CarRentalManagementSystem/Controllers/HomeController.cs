using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRentalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // -------------------
        // Home Page (Guest / Customer)
        // -------------------
        public IActionResult Index()
        {
            // 🚫 Redirect Admin to Admin Dashboard
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            // Fetch available cars to show on homepage
            var cars = _context.Cars
                               .Where(c => c.IsAvailable) // only available cars
                               .OrderBy(c => c.CarName)
                               .ToList();

            return View(cars); // Pass cars to Index.cshtml
        }

        // -------------------
        // Privacy Page
        // -------------------
        public IActionResult Privacy()
        {
            // 🚫 Block Admin from seeing public pages
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return View();
        }

        // -------------------
        // Error Page
        // -------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}