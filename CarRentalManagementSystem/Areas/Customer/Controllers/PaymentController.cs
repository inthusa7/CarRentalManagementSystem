using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var payments = await _context.Payments
                .Include(p => p.Booking)
                .Where(p => p.Booking.UserID == customerId.Value)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return View(payments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int bookingId)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == bookingId && b.UserID == customerId.Value);
            if (booking == null) return NotFound();

            ViewBag.BookingId = booking.BookingID;
            ViewBag.Amount = booking.TotalCost;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookingId, decimal amount, string paymentMethod)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == bookingId && b.UserID == customerId.Value);
            if (booking == null) return NotFound();

            var payment = new Payment
            {
                BookingID = bookingId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Booking", new { area = "Customer" });
        }
    }
}
