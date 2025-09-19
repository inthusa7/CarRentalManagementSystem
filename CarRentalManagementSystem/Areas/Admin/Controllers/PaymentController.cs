using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                            .Include(p => p.Booking)
                            .ThenInclude(b => b.User)
                            .Include(p => p.Booking.Car)
                            .ToList();
            return View(payments);
        }
        // GET: Admin/Payment/Details/5
        public IActionResult Details(int id)
        {
            var payment = _context.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.User)
                .Include(p => p.Booking.Car)
                .FirstOrDefault(p => p.PaymentID == id);

            if (payment == null) return NotFound();

            return View(payment);
        }
    }
}
