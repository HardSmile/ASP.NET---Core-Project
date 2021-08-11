
namespace Project.Services.Cars
{
    using Project.Models;
    using Project.Models.Cars;
    using System.Collections.Generic;

    public interface ICarService
    {
        CarQueryServiceModel All(
            string brand,
            string searchTerm,
            CarSorting sorting,
            int currentPage,
            int carsPerPage);
        CarDetailsServiceModel Details(int id);
        int Create(
            string brand,
               string model,
                string description,
                int year,
               string imageUrl,
               int categoryId,
                int dealerId);
        bool Edit(
            int id,
            string brand,
               string model,
                string description,
                int year,
               string imageUrl,
               int categoryId
                );
        IEnumerable<CarServiceModel> ByUser(string userId);
        bool IsByDealer(int carId, int dealerId);
        IEnumerable<string> AllCarsBrands();
        IEnumerable<CarCategoryServiceModel> AllCarCategories();
        bool CategoryExist(int categoryId);
    }
}
