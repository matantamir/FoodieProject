using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FoodieProject.Models
{
    public class Restaurant
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Address AddressId { get; set; }

        [Display(Name = "Average Price")]
        [Range(1,3)]
        public int AveragePrice { get; set; }
        
        public IFormFile Picture { get; set; }

        [Range(1, 5)]
        public int Rate { get; set; }

        public About About { get; set; }  // one to one (one About to one Restaurant)

        public int AboutID { get; set; } // one to one (one About to one Restaurant) - this declare a foreign key

        public List<Dish> Dishes { get; set; } // many to one (many Dishes to one Restaurant)

        public List<Tag> Tags { get; set; } // many to many (many Tags to many Restaurant)
    }
}
