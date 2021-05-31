using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class Dish
    {

        [Key]
        public int DishID { get; set; }

        [Required]
        public int DishName { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string Ingredients { get; set; }

        public int Rate { get; set; }

        // What type for picture?
        public int Picture { get; set; }
        
        public int RestID { get; set; }  // many to one (many Dishs to one Restaurant) - this declare a foreign key

        public Restaurant Restaurant { get; set; }  // many to one (many Dishs to one Restaurant)
    }
}
