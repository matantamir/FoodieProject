using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class DishTag
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "Dish Tag Name")]
        public int Name { get; set; }

        public List<Dish> Dishes { get; set; }  // many to many (many Allergens to many Dishes)

    }
}
