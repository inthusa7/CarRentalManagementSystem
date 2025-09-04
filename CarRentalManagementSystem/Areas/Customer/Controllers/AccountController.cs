using CarRentalManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Login()
        {
            return View();
        }
        // POST: Customer/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var customer = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == "Customer");
            if (customer != null)
            {
                HttpContext.Session.SetString("Username", customer.Username);
                HttpContext.Session.SetInt32("UserID", customer.UserID);
                return RedirectToAction("Index", "Car");
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
