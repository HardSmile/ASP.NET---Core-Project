using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.Cars
{
    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 3;
        public string Brand { get;set; }
        public IEnumerable<string> Brands { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; init; }
        public CarSorting Sorting { get; init; }
        public int CurrentPage { get; init; } = 1;
        public int TotalCars { get; set; }

        public IEnumerable<CarListingViewModel> Cars { get; set; }
    }
}
