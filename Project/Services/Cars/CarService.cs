
namespace Project.Services.Cars
{
using Project.Data;
using Project.Models;
using System.Collections.Generic;
using System.Linq;

    public class CarService : ICarService
    {
        private readonly CarRentingDbContext data;


        public CarService(CarRentingDbContext data)=>this.data = data;
             public CarQueryServiceModel All(
            string brand,
            string searchTerm,
            CarSorting sorting,
            int currentPage ,
            int carsPerPage)
        {
            var carsQuery = this.data.Cars.AsQueryable();
            if (!string.IsNullOrWhiteSpace(brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == brand);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                carsQuery = carsQuery.Where(c => c.Brand.ToLower().Contains(searchTerm.ToLower())
                    || c.Model.ToLower().Contains(searchTerm.ToLower())
                    || c.Descriptiom.ToLower().Contains(searchTerm.ToLower()));
            }
            carsQuery = sorting switch
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
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage)
                .Select(c => new CarServiceModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();
            return new CarQueryServiceModel
            {
                TotalCars = totalCars,
                CurrentPage = currentPage,
                CarsPerPage = carsPerPage,
                Cars = cars
            };
        }
        public IEnumerable<string> AllCarsBrands() => 
            this.data
            .Cars
            .Select(c => c.Brand)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

    }
}
