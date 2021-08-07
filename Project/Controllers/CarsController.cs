
namespace Project.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Project.Data;
    using Project.Data.Models;
    using Project.Infrastructure;
    using Project.Models.Cars;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data) => this.data = data;



        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var carsQuery = this.data.Cars.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == query.Brand);
            }
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                carsQuery = carsQuery.Where(c => c.Brand.ToLower().Contains(query.SearchTerm.ToLower())
                    || c.Model.ToLower().Contains(query.SearchTerm.ToLower())
                    || c.Descriptiom.ToLower().Contains(query.SearchTerm.ToLower()));
            }
            carsQuery = query.Sorting switch
            {
                CarSorting.DateCreated => carsQuery
                .OrderByDescending(c => c.Id),
                CarSorting.Year => carsQuery
                 .OrderByDescending(c => c.Year),
                CarSorting.BrandAndModel => carsQuery
                .OrderByDescending(c => c.Brand)
                .ThenBy(c => c.Model)
            };
            var totalCars = carsQuery.Count();
            var cars = carsQuery
                .Skip((query.CurrentPage - 1) * AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
                .Select(c => new CarListingViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();
            var carBrands = this.data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .ToList();
            query.Brands = carBrands;
            query.Cars = cars;
            query.TotalCars = totalCars;
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
