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

        // GET: Customer/Car
        public IActionResult Index()
        {
            var cars = _context.Cars
                       .Where(c => c.IsAvailable)
                       .ToList();
            return View(cars); // ✅ pass cars to view
        }

        // GET: Customer/Car/Details/5
        public IActionResult Details(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car == null) return NotFound();

            return View(car);
        }
    }
}
