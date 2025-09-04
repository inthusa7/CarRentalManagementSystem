using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Customer
{
    [Area("Customer")]
    public class UserBookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/UserBooking/BookNow/5
        public IActionResult BookNow(int carId)
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Account", new { area = "Customer" });

            var car = _context.Cars.Find(carId);
            if (car == null || !car.IsAvailable)
                return NotFound();

            var booking = new Booking
            {
                CarID = carId,
                UserID = int.Parse(userIdString),
                PickupDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(1)
            };

            ViewBag.Car = car;
            return View(booking);
        }

        // POST: Customer/UserBooking/BookNow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookNow(Booking model)
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Account", new { area = "Customer" });

            model.UserID = int.Parse(userIdString);

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
                days = days == 0 ? 1 : days;
                model.TotalCost = days * car.DailyRate;

                car.IsAvailable = false;

                _context.Bookings.Add(model);
                _context.SaveChanges();

                return RedirectToAction("History", "UserBooking", new { area = "Customer" });
            }

            ViewBag.Car = _context.Cars.Find(model.CarID);
            return View(model);
        }

        // GET: Customer/UserBooking/History
        public IActionResult History()
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Account", new { area = "Customer" });

            int userId = int.Parse(userIdString);

            var bookings = _context.Bookings
                                   .Include(b => b.Car)
                                   .Where(b => b.UserID == userId)
                                   .OrderByDescending(b => b.PickupDate)
                                   .ToList();

            return View(bookings);
        }

        // POST: Customer/UserBooking/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Account", new { area = "Customer" });

            int userId = int.Parse(userIdString);

            var booking = _context.Bookings
                                  .Include(b => b.Car)
                                  .FirstOrDefault(b => b.BookingID == id && b.UserID == userId);

            if (booking == null)
                return NotFound();

            if (booking.PickupDate > DateTime.Now)
            {
                booking.Car.IsAvailable = true;
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }

            return RedirectToAction("History");
        }
    }
}


