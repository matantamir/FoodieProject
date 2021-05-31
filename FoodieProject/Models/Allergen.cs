using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class Allergen
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Name { get; set; }

        public List<Dish> Dishes { get; set; }  // many to many (many Allergens to many Dishes)

    }
}
