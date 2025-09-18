using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Customer/Booking/Create?carId=1
        public async Task<IActionResult> Create(int carId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarID == carId);
            if (car == null) return NotFound();

            var vm = new BookingCreateViewModel
            {
                CarID = car.CarID,
                CarName = car.CarName,
                CarModel = car.CarModel,
                DailyRate = car.DailyRate,
                PickupDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(1)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel vm)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            if (!ModelState.IsValid) return View(vm);

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarID == vm.CarID);
            if (car == null) return NotFound();

            if (!car.IsAvailable)
            {
                ModelState.AddModelError("", "This car is currently not available.");
                return View(vm);
            }

            if (vm.ReturnDate.Date <= vm.PickupDate.Date)
            {
                ModelState.AddModelError("", "Return date must be after pickup date.");
                return View(vm);
            }

            int days = (vm.ReturnDate.Date - vm.PickupDate.Date).Days;
            if (days < 1) days = 1;

            var booking = new Booking
            {
                CarID = car.CarID,
                UserID = customerId.Value,
                PickupDate = vm.PickupDate,
                ReturnDate = vm.ReturnDate,
                TotalCost = days * car.DailyRate
            };

            _context.Bookings.Add(booking);
            car.IsAvailable = false;
            _context.Cars.Update(car);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: My bookings
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

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var booking = await _context.Bookings.Include(b => b.Car)
                .FirstOrDefaultAsync(b => b.BookingID == id && b.UserID == customerId.Value);

            if (booking == null) return NotFound();

            var car = booking.Car;

            _context.Bookings.Remove(booking);
            if (car != null)
            {
                car.IsAvailable = true;
                _context.Cars.Update(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
