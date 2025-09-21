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
        public async Task<IActionResult> Create(Car car, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle Image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cars", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    car.ImageUrl = "images/cars/" + fileName;
                }
                else
                {
                    car.ImageUrl = "images/cars/default-car.png"; // default placeholder
                }

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, return the same view with error messages
            return View(car);
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
        public async Task<IActionResult> Edit(int id, Car car, IFormFile? ImageFile)
        {
            if (id != car.CarID) return NotFound();

            if (ModelState.IsValid)
            {
                var existingCar = _context.Cars.Find(id);
                if (existingCar == null) return NotFound();

                // Update basic details
                existingCar.CarName = car.CarName;
                existingCar.CarModel = car.CarModel;
                existingCar.DailyRate = car.DailyRate;
                existingCar.SeatCount = car.SeatCount;
                existingCar.CarColor = car.CarColor;
                existingCar.Location = car.Location;
                existingCar.IsAvailable = car.IsAvailable;

                // Update image only if a new file is uploaded
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cars", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    existingCar.ImageUrl = "images/cars/" + fileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(car);
        }


        // POST: Admin/Car/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
