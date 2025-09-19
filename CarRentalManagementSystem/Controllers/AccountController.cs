using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarRentalManagementSystem.Models;
using CarRentalManagementSystem.Data;
using System.Linq;

namespace CarRentalManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Fixed Admin credentials
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------- Register (Customer Only) --------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(model);
                }

                model.Role = "Customer"; // Always Customer
                _context.Users.Add(model);
                _context.SaveChanges();

                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // -------- Login (Admin + Customer) --------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // --- Admin Check ---
                if (model.Username == AdminUsername && model.Password == AdminPassword)
                {
                    HttpContext.Session.SetString("Username", AdminUsername);
                    HttpContext.Session.SetString("Role", "Admin");

                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                // --- Customer Check ---
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", "Customer");
                    HttpContext.Session.SetInt32("CustomerID", user.UserID);

                    return RedirectToAction("Index", "Dashboard", new { area = "Customer" });
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        // -------- Logout --------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
