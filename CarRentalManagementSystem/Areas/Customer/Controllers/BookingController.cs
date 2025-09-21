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

        // ---------------- Available Cars ----------------
        public async Task<IActionResult> AvailableCars()
        {
            var cars = await _context.Cars
                .Where(c => c.IsAvailable)
                .ToListAsync();
            return View(cars);
        }

        // ---------------- BookNow GET ----------------
        public async Task<IActionResult> BookNow(int carId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarID == carId);
            if (car == null) return NotFound();

            var user = await _context.Users.FindAsync(customerId.Value);

            var vm = new BookingCreateViewModel
            {
                CarID = car.CarID,
                CarName = car.CarName,
                CarModel = car.CarModel,
                CarColor = car.CarColor,
                SeatCount = car.SeatCount,
                Location = car.Location,
                Description = car.Description,
                DailyRate = car.DailyRate,
                ImageUrl = car.ImageUrl,
                PickupDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(1),
                CustomerName = user != null ? user.Username : ""
            };

            return View(vm);
        }

        // ---------------- BookNow POST ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookNow(BookingCreateViewModel vm, string action)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            // Cancel button clicked on BookNow page
            if (action == "Cancel")
                return RedirectToAction("AvailableCars");

            if (!ModelState.IsValid)
                return View(vm);

            if (vm.ReturnDate <= vm.PickupDate)
            {
                ModelState.AddModelError("", "Return date must be after Pickup date.");
                return View(vm);
            }

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarID == vm.CarID);
            if (car == null || !car.IsAvailable)
            {
                ModelState.AddModelError("", "Selected car is not available.");
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
                TotalCost = days * car.DailyRate,
                IsPaid = false
            };

            _context.Bookings.Add(booking);

            // Make car unavailable
            car.IsAvailable = false;
            _context.Cars.Update(car);

            await _context.SaveChangesAsync();

            // Redirect to Payment page after booking
            return RedirectToAction("Create", "Payment", new { area = "Customer", bookingId = booking.BookingID });
        }

        // ---------------- My Bookings ----------------
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

        // ---------------- Cancel Booking ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var booking = await _context.Bookings
                .Include(b => b.Car)
                .FirstOrDefaultAsync(b => b.BookingID == id && b.UserID == customerId.Value);

            if (booking == null)
                return NotFound();

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
