﻿namespace Project.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.Dealer;
    public class Dealer
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(DealerNameMaxLength)]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string UserId { get; set; }

        public IEnumerable<Car> Cars { get; init; } = new List<Car>();
    }
}
