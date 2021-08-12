
namespace Project.Controllers.Api
{
using Microsoft.AspNetCore.Mvc;
    using Project.Controllers.Api.Cars;

    using Project.Services.Cars;
    using Project.Services.Cars.Models;

    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly ICarService cars;

        public CarsApiController(ICarService data)
        => this.cars = cars;
        [HttpGet]
        public  CarQueryServiceModel All([FromQuery] AllCarsApiRequestModel query)
        {
            return this.cars.All(
                 query.Brand,
                 query.SearchTerm,
                 query.Sorting,
                 query.CurrentPage,
                 query.CarsPerPage
                );

         
        }


    }
}
