using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View list of users (Admin only)
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account", new { area = "" });

            // Fetch all users from DB
            var users = _context.Users.AsNoTracking().ToList();
            return View(users);
        }

        // View user details (Admin only, read-only)
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account", new { area = "" });

            if (id == null) return NotFound();

            // Always get fresh data from DB
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.UserID == id);
            if (user == null) return NotFound();

            return View(user); // Admin can view, cannot edit
        }
    }
}
