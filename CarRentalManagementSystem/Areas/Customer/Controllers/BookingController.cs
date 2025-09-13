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

        // GET: Customer/Booking/BookNow/5
        public IActionResult BookNow(int carId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var car = _context.Cars.Find(carId);
            if (car == null || !car.IsAvailable)
                return NotFound();

            var booking = new Booking
            {
                CarID = carId,
                CustomerID = customerId.Value,
                PickupDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(1)
            };

            ViewBag.Car = car;
            return View(booking);
        }

        // POST: Customer/Booking/BookNow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookNow(Booking model)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            model.CustomerID = customerId.Value;

            if (ModelState.IsValid)
            {
                var car = _context.Cars.Find(model.CarID);
                if (car == null || !car.IsAvailable)
                {
                    ModelState.AddModelError("", "Car is not available.");
                    ViewBag.Car = car;
                    return View(model);
                }

                var days = (model.ReturnDate - model.PickupDate).Days;
                if (days <= 0) days = 1;
                model.TotalCost = days * car.DailyRate;

                car.IsAvailable = false;

                _context.Bookings.Add(model);
                _context.SaveChanges();

                return RedirectToAction("History");
            }

            ViewBag.Car = _context.Cars.Find(model.CarID);
            return View(model);
        }

        // GET: Customer/Booking/History
        public IActionResult History()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var bookings = _context.Bookings
                                   .Include(b => b.Car)
                                   .Where(b => b.CustomerID == customerId.Value)
                                   .OrderByDescending(b => b.PickupDate)
                                   .ToList();

            return View(bookings);
        }

        // POST: Customer/Booking/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account", new { area = "" });

            var booking = _context.Bookings
                                  .Include(b => b.Car)
                                  .FirstOrDefault(b => b.BookingID == id && b.CustomerID == customerId.Value);

            if (booking == null)
                return NotFound();

            if (booking.Car != null)
                booking.Car.IsAvailable = true;

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return RedirectToAction("History");
        }
    }
}
