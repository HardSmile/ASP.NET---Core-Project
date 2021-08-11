
namespace Project.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Mvc;
    using Project.Data;
    using Project.Infrastructure;
    using Project.Models.Cars;
    using Project.Services.Cars;
    using Project.Services.Dealers;



    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IDealerService dealers;
        public CarsController(CarRentingDbContext data, ICarService cars, IDealerService dealers)
        {
            this.cars = cars;
            this.dealers = dealers;
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
        public IActionResult Mine()
        {
            var myCars = this.cars.ByUser(this.User.GetId());
            return View(myCars);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.dealers.IsDealer(this.User.GetId()))
            {
               
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }
            else
            {

            return View(new CarFormModel
        {
                Categories = this.cars.AllCarCategories()
        });

            }
                
    }
        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car)
        {
            var dealerId = this.dealers.IdUser(this.User.GetId());
            if(dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }
               
          

            if (!this.cars.CategoryExist(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = this.cars.AllCarCategories();
                return View(car);
            }
            this.cars.Create(
                car.Brand,
                car.Model,
                car.Description,
                car.Year,
                car.ImageUrl,
                car.CategoryId,
                dealerId);
            return RedirectToAction(nameof(All));
            
        }
        [Authorize]
        public IActionResult Edit (int id)
        {

            if (!this.dealers.IsDealer(this.User.GetId()))
            {

                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }
            var car = this.cars.Details(id);
            if(car.UserId != this.User.GetId())
            {
                return Unauthorized();
            }

                return View(new CarFormModel
                {Brand=car.Brand,
                Model = car.Model,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                Year = car.Year,
                CategoryId = car.CategoryId,

                    Categories = this.cars.AllCarCategories()
                });
            }

     [Authorize]
     [HttpPost]
     public IActionResult Edit (int id, CarFormModel car)
        {
            var dealerId = this.dealers.IdUser(this.User.GetId());
            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Create), "Dealers");
            }



            if (!this.cars.CategoryExist(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = this.cars.AllCarCategories();
                return View(car);
            }
            if (!this.cars.IsByDealer(id,dealerId))
            {
                return BadRequest();
            }
            this.cars.Edit(
                id,
                car.Brand,
                car.Model,
                car.Description,
                car.Year,
                car.ImageUrl,
                car.CategoryId
                );
        
            return RedirectToAction(nameof(All));
        }
  
    }
}
