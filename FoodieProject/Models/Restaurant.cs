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
        public int RestID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        // Maybe we want Address CLASS? for city, street, no. ?
        public string Address { get; set; }

        public int AveragePrice { get; set; }
        
        // What type for picture?
        public int Picture { get; set; }

        public About About { get; set; }  // one to one (one About to one Restaurant)

        public int AboutID { get; set; } // one to one (one About to one Restaurant) - this declare a foreign key

        public List<Dish> Dishs { get; set; } // many to one (many Dishs to one Restaurant)

        public List<Tag> Tags { get; set; } // many to many (many Tags to many Restaurant)
    }
}
