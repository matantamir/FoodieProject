using Microsoft.AspNetCore.Http;
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
        public int Id { get; set; }

        [Required, Display(Name = "Dish Name")]
        public int Name { get; set; }

        [Display(Name = "Dish Description")]
        public string Description { get; set; }

        [Required, DataType(DataType.Currency),Display(Name ="Dish Price")]
        public int Price { get; set; }

        [Display(Name = "Dish Picture")]
        public IFormFile Picture { get; set; }

        [Display(Name = "Dish Tag")]
        public List<DishTag> DishTags { get; set; } // many to many (many Allergens to many Dishes)

        public int RestID { get; set; }  // many to one (many Dishs to one Restaurant) - this declare a foreign key

        public Restaurant Restaurant { get; set; }  // many to one (many Dishs to one Restaurant)
    }
}
