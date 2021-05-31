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

        [Required]
        [Display(Name = "Dish Name")]
        public int Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        public string Picture { get; set; }

        public List<Allergen> Allergens { get; set; } // many to many (many Allergens to many Dishes)

        public int RestID { get; set; }  // many to one (many Dishs to one Restaurant) - this declare a foreign key

        public Restaurant Restaurant { get; set; }  // many to one (many Dishs to one Restaurant)
    }
}
