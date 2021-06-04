using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "City")]
        public int City { get; set; }

        [Required, Display(Name = "Street")]
        public int Street { get; set; }

        [Required, Display(Name = "Number")]
        public int Number { get; set; }

        [Display(Name = "Map Latitude")]
        public int MapLatitude { get; set; }

        [Display(Name = "Map Longitude")]
        public int MapLongitude { get; set; }
    }
}
