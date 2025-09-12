using CarRentalManagementSystem.Data; // Adjust namespace
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
                return RedirectToAction("Index", "Cars"); // Navigate to car listing
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // =====================
        // Customer Registration
        // =====================
        public IActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                model.Role = "Customer"; // Set role
                _context.Users.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Registration Successful! You can now login.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}