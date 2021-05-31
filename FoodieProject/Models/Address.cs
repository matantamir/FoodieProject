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

        [Required]
        public int City { get; set; }

        [Required]
        public int Street { get; set; }

        [Required]
        public int Number { get; set; }

        public int MapLatitude { get; set; }

        public int MapLongitude { get; set; }
    }
}
