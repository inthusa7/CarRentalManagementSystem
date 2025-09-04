using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        // POST: Admin/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == "Admin");
            if (admin != null)
            {
                HttpContext.Session.SetString("AdminUsername", admin.Username);
                HttpContext.Session.SetInt32("AdminID", admin.UserID);
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
