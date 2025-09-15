using Microsoft.AspNetCore.Mvc;
using CarRentalManagementSystem.Models;
using CarRentalManagementSystem.Data; // Your DbContext namespace
using System.Linq;

namespace CarRentalManagementSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Car/Index
        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }

        // GET: Car/Details/5
        public IActionResult Details(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car == null) return NotFound();
            return View(car);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Car/Edit/5
        public IActionResult Edit(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car == null) return NotFound();
            return View(car);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Update(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Car/Delete/5
        public IActionResult Delete(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car == null) return NotFound();
            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
