using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class Restaurant
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; }

        [Required, Display(Name = "Restaurant Address")]
        public Address AddressId { get; set; }

        [Display(Name = "Average Price"), Range(1, 3)]
        public int AveragePrice { get; set; }

        [Display(Name = "Restaurant Picture")]
        public string PicturePath { get; set; }

        [Range(1, 5), Display(Name = "Restaurant Rate")]
        public int Rate { get; set; }

        [Display(Name = "Restaurant About")]
        public About About { get; set; }  // one to one (one About to one Restaurant)

        public int AboutID { get; set; } // one to one (one About to one Restaurant) - this declare a foreign key

        [Display(Name = "Restaurant Dishes")]
        public List<Dish> Dishes { get; set; } // many to one (many Dishes to one Restaurant)

        [Display(Name = "Restaurant Tags")]
        public List<Tag> Tags { get; set; } // many to many (many Tags to many Restaurant)
    }
}
