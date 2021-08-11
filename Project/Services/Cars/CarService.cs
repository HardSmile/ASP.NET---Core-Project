
namespace Project.Services.Cars
{
using Project.Data;
    using Project.Data.Models;
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
                    || c.Description.ToLower().Contains(searchTerm.ToLower()));
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
            var cars = GetCars(carsQuery
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage));
                
            return new CarQueryServiceModel
            {
                TotalCars = totalCars,
                CurrentPage = currentPage,
                CarsPerPage = carsPerPage,
                Cars = cars
            };
        }
        public CarDetailsServiceModel Details(int id)
      => this.data.Cars
            .Where(d => d.Id == id)
            .Select(c => new CarDetailsServiceModel
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Description = c.Description,
                Year = c.Year,
                ImageUrl = c.ImageUrl,
                Category = c.Category.Name,
               DeakerId = c.DealerId,
               DealerName = c.Dealer.Name,
               UserId = c.Dealer.UserId
                
                
            })
            .FirstOrDefault();

        public int Create(string brand, string model, string description, int year, string imageUrl, int categoryId, int dealerId)
        {
            var carData = new Car
            {
                Brand = brand,
                Model = model,
                Description = description,
                Year = year,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                DealerId = dealerId
            };
            this.data.Cars.Add(carData);
            this.data.SaveChanges();
            return carData.Id;
        }
        public bool Edit(int id,string brand, string model, string description, int year, string imageUrl, int categoryId)
        {
            var carData = this.data.Cars.Find(id);
     if(carData == null)
            {
                return false;
            }

            carData.Id = id;
            carData.Brand = brand;
            carData.Model = model;
            carData.Description = description;
            carData.Year = year;
            carData.ImageUrl = imageUrl;
            carData.CategoryId = categoryId;
      
        
            this.data.SaveChanges();
            return true;
        }


        public IEnumerable<CarServiceModel> ByUser(string userId)
      => this.GetCars(this.data
          .Cars
          .Where(c => c.Dealer.UserId == userId));
        public bool IsByDealer(int carId, int dealerId)
=> data
            .Cars
            .Any(c => c.Id == carId && c.DealerId == dealerId); 
        public IEnumerable<string> AllCarsBrands() => 
            this.data
            .Cars
            .Select(c => c.Brand)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
        public bool CategoryExist(int categoryId)
        =>
            this.data.Categories
            .Any(c => c.Id == categoryId);
        

        private IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carQuery)
            =>carQuery
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

        public IEnumerable<CarCategoryServiceModel> AllCarCategories()
        
                     => this.data
            .Categories
            .Select(c => new CarCategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();


    }
}
