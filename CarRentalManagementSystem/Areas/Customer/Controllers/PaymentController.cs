using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var payments = _context.Payments
               .OrderByDescending(p => p.PaymentDate)
               .ToList();
            return View(payments);
            
        }
        // Show payment form
        [HttpGet]
        public IActionResult Create(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingID == bookingId);
            if (booking == null) return NotFound();

            ViewBag.BookingId = booking.BookingID;
            ViewBag.Amount = booking.TotalCost;
            return View();
        }

        // Save payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int bookingId, decimal amount, string paymentMethod)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingID == bookingId);
            if (booking == null) return NotFound();

            var payment = new Payment
            {
                BookingID = bookingId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            // Redirect to booking history after payment
            return RedirectToAction("History", "Booking");
        }
    }
}


