using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserCarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCarController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cars = _context.Cars
                       .Where(c => c.IsAvailable)
                       .ToList();
            return View();
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


       