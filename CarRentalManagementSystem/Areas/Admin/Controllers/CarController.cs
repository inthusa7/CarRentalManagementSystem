using CarRentalManagementSystem.Data;
using CarRentalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Car
        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }

        // GET: Admin/Car/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car model)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Car/Edit/5
        public IActionResult Edit(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();
            return View(car);
        }

        // POST: Admin/Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car model)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Car/Delete/5
        public IActionResult Delete(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();
            return View(car);
        }

        // POST: Admin/Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
