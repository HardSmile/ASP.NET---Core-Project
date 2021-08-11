
namespace Project.Models.Cars
{
    using Project.Services.Cars;
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Data.DataConstants.Car;
    public class CarFormModel
    {
        [Required]
        [StringLength(CarBrandMaxLength,MinimumLength = CarBrandMinLength)]
      
        public string Brand { get; init; }
        [Required]
        [StringLength(CarModelMaxLength,MinimumLength =CarModelMinLength)]
         public string Model { get; init; }
    [Required]
    [StringLength(int.MaxValue,MinimumLength = CarDescriptionMinLength , ErrorMessage ="The field Description must be a string with a minimum of {1}.")]
        public string Description { get; init; }
         [Display(Name = "Image URL")]
         [Required]
         [Url]
        public string ImageUrl { get; init; }
        [Range(CarYearMinLength,CarYearMaxLength)]
        public int Year { get; init ; }
        [Display(Name = "Categorty")]
        public int CategoryId { get; init; }
        public IEnumerable<CarCategoryServiceModel> Categories { get; set; }
     

    }
}
