using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // show only available cars
            var cars = _context.Cars.Where(c => c.IsAvailable).ToList();
            return View(cars);
        }

        public IActionResult Details(int id)
        {
            // require user logged in
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car == null) return NotFound();

            return View(car);
        }
    }
}