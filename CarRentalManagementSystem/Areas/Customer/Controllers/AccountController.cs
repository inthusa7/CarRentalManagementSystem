using Microsoft.AspNetCore.Mvc;
using CarRentalManagementSystem.Data; // Adjust namespace
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================
        // Customer Login
        // =====================
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var customer = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == "Customer");

            if (customer != null)
            {
                HttpContext.Session.SetString("Username", customer.Username);
                HttpContext.Session.SetInt32("UserID", customer.UserID);
                return RedirectToAction("Index", "Car");
            }

            ViewBag.Error = "Invalid username or password";
            ViewData["Title"] = "Login";
            return View();
        }

        // =====================
        // Customer Sign In
        // =====================
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(string username, string password)
        {
            var customer = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == "Customer");

            if (customer != null)
            {
                HttpContext.Session.SetString("Username", customer.Username);
                HttpContext.Session.SetInt32("UserID", customer.UserID);
                return RedirectToAction("Index", "Car");
            }

            ViewBag.Error = "Invalid username or password";
            ViewData["Title"] = "Sign In";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
