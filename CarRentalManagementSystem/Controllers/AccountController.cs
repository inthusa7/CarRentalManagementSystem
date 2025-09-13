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

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------- Register (Customer only) --------
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                model.Role = "Customer";

                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(model);
                }

                _context.Users.Add(model);
                _context.SaveChanges();

                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // -------- Login --------
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in → redirect
            var role = HttpContext.Session.GetString("Role");
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin")
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                else
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = null;

                // 🔹 Hard-coded Admin login
                if (model.Username == "admin" && model.Password == "admin123")
                {
                    user = new User
                    {
                        UserID = 0,
                        Username = "admin",
                        Role = "Admin",
                        Email = "admin@example.com"
                    };
                }
                else
                {
                    // 🔹 Normal Customer login
                    user = _context.Users
                        .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                }

                if (user != null)
                {
                    // Save session
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetInt32("CustomerID", user.UserID);

                    // Prevent cache
                    Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    Response.Headers["Pragma"] = "no-cache";
                    Response.Headers["Expires"] = "0";

                    // ✅ Redirect based on Role
                    if (user.Role == "Admin")
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        // -------- Logout --------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            // Prevent back after logout
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Login");
        }
    }
}
