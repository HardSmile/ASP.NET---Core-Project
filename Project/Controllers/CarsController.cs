
namespace Project.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Mvc;
    using Project.Data;
    using Project.Data.Models;
    using Project.Infrastructure;

    using Project.Models.Cars;
    using Project.Services.Cars;
    using System.Collections.Generic;
    using System.Linq;


    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly ICarService cars;
        public CarsController(CarRentingDbContext data, ICarService cars) {
            this.data = data;
            this.cars = cars;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = this.cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);
            var carBrands = this.cars.AllCarsBrands();
            query.Brands = carBrands;
            query.TotalCars = queryResult.TotalCars;
            query.Cars = queryResult.Cars;
            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsDealer())
            {
               
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }
            else
            {

            return View(new AddCarFormModel
        {
                Categories = this.GetCarCategories()
        });

            }
                
    }
        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car)
        {
            var dealerId = this.data
                .Dealers
                .Where(d => d.UserId == User.GetId())
                .Select(d => d.Id)
                .FirstOrDefault();
            if(dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }
               
          

            if (!this.data.Categories.Any(c=>c.Id == car.CategoryId))
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
                CategoryId = car.CategoryId,
                DealerId = dealerId

            };
            this.data.Cars.Add(carData);
            this.data.SaveChanges();
            return RedirectToAction(nameof(All));
            
        }
        private bool UserIsDealer() =>
            this.data
            .Dealers
            .Any(d => d.UserId == this.User.GetId());
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
