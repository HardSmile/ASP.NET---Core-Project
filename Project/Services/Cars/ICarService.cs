
namespace Project.Services.Cars
{
    using Project.Models;
    using System.Collections.Generic;

    public interface ICarService
    {
        CarQueryServiceModel All(
            string brand,
            string searchTerm,
            CarSorting sorting,
            int currentPage,
            int carsPerPage);
        IEnumerable<string> AllCarsBrands();
    }
}
