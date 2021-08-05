
namespace Project.Controllers
{
using Microsoft.AspNetCore.Mvc;
    using Project.Data;
    using Project.Data.Models;
    using Project.Models.Cars;
    using System.Collections.Generic;
    using System.Linq;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)=>this.data = data;


        public IActionResult Add() => View(new AddCarFormModel
        {
            Categories = this.GetCarCategories()
        });
        [HttpPost]
        public IActionResult Add(AddCarFormModel car)
        {
            if(!this.data.Categories.Any(c=>c.Id == car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = this.GetCarCategories();
                return View(car);
            }
            var carData = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Descriptiom = car.Description,
                Year = car.Year,
                ImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId

            };
            this.data.Cars.Add(carData);
            this.data.SaveChanges();
            return RedirectToAction("Index", "Home");
            
        }
        private IEnumerable<CarCategoryViewModel> GetCarCategories()
            => this.data
            .Categories
            .Select(c => new CarCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();
    }
}
