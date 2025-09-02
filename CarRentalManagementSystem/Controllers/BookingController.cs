using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;

namespace CarRentalManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Booking/BookNow/5
        public IActionResult BookNow(int carId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Account");

            var car = _context.Cars.Find(carId);
            if (car == null || !car.IsAvailable)
                return NotFound();

            var booking = new Booking
            {
                CarID = carId,
                CustomerID = int.Parse(userIdString)
            };

            ViewBag.Car = car;
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookNow(Booking model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Account");

            model.CustomerID = int.Parse(userIdString);

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
                return RedirectToAction("Index", "Cars");
            }

            ViewBag.Car = _context.Cars.Find(model.CarID);
            return View(model);
        }
    }
}
