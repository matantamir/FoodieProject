using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodieProject.Models
{
    public class Restaurant
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber), RegularExpression("[0-9-]{5,20}", ErrorMessage = "Phone number must match the form : 050-1234567 or 03-1234567"), Display(Name = "Restaurant Phone")]
        public string Phone { get; set; }

        public int AddressId { get; set; } // one to one (one Address to one Restaurant) - this declare a foreign key

        [Display(Name = "Restaurant Address")]
        public Address Address { get; set; } // one to one (one Address to one Restaurant)

        [Display(Name = "Average Price"), Range(1, 3)]
        public int AveragePrice { get; set; }

        [Display(Name = "Restaurant Picture")]
        public string PicturePath { get; set; }

        [Range(1, 5), Display(Name = "Restaurant Rate")]
        public int Rate { get; set; }

        [Display(Name = "Restaurant About")]
        public string About { get; set; }  

        [Display(Name = "Restaurant Dishes")]
        public List<Dish> Dishes { get; set; } // many to one (many Dishes to one Restaurant)

        [Display(Name = "Restaurant Tags")]
        public List<Tag> Tags { get; set; } // many to many (many Tags to many Restaurant)
    }
}
