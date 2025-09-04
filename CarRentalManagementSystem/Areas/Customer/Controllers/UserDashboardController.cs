using Microsoft.AspNetCore.Mvc;

namespace CarRentalManagementSystem.Areas.Customer.Controllers
{
    public class UserDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
