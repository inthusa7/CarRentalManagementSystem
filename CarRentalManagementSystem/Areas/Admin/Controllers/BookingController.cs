using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // for session

namespace CarRentalManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================== Admin Actions ==================

        // GET: Admin/Booking
        public IActionResult Index()
        {
            var bookings = _context.Bookings
                          .Include(b => b.User)
                          .Include(b => b.Car)
                          .ToList();
            return View(bookings);
        }

        // GET: Admin/Booking/Create
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Cars = _context.Cars.Where(c => c.IsAvailable).ToList();
            return View();
        }

        // POST: Admin/Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking model)
        {
            if (ModelState.IsValid)
            {
                var car = _context.Cars.Find(model.CarID);
                if (car == null || !car.IsAvailable)
                {
                    ModelState.AddModelError("CarID", "Selected car is not available.");
                    ViewBag.Users = _context.Users.ToList();
                    ViewBag.Cars = _context.Cars.Where(c => c.IsAvailable).ToList();
                    return View(model);
                }

                // Calculate total cost
                var days = (model.ReturnDate - model.PickupDate).Days;
                days = days == 0 ? 1 : days;
                model.TotalCost = days * car.DailyRate;

                // Mark car unavailable
                car.IsAvailable = false;

                _context.Bookings.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Users = _context.Users.ToList();
            ViewBag.Cars = _context.Cars.Where(c => c.IsAvailable).ToList();
            return View(model);
        }

        // GET: Admin/Booking/Edit/5
        public IActionResult Edit(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
                return NotFound();

            ViewBag.Users = _context.Users.ToList();
            ViewBag.Cars = _context.Cars
                            .Where(c => c.IsAvailable || c.CarID == booking.CarID)
                            .ToList();

            return View(booking);
        }

        // POST: Admin/Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Booking model)
        {
            if (ModelState.IsValid)
            {
                var oldBooking = _context.Bookings.AsNoTracking().FirstOrDefault(b => b.BookingID == model.BookingID);
                if (oldBooking == null)
                    return NotFound();

                // If car changed, update availability
                if (oldBooking.CarID != model.CarID)
                {
                    var oldCar = _context.Cars.Find(oldBooking.CarID);
                    if (oldCar != null) oldCar.IsAvailable = true;

                    var newCar = _context.Cars.Find(model.CarID);
                    if (newCar != null) newCar.IsAvailable = false;
                }

                // Calculate total cost
                var car = _context.Cars.Find(model.CarID);
                var days = (model.ReturnDate - model.PickupDate).Days;
                days = days == 0 ? 1 : days;
                model.TotalCost = days * car.DailyRate;

                _context.Bookings.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Users = _context.Users.ToList();
            ViewBag.Cars = _context.Cars.ToList();
            return View(model);
        }

        // GET: Admin/Booking/Delete/5
        public IActionResult Delete(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Car)
                .FirstOrDefault(b => b.BookingID == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Admin/Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
                return NotFound();

            // Make the car available again
            var car = _context.Cars.Find(booking.CarID);
            if (car != null) car.IsAvailable = true;

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
