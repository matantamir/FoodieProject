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

        [Required, Display(Name = "City")]
        public string City { get; set; }

        [Required, Display(Name = "Street")]
        public string Street { get; set; }

        [Required, Display(Name = "Number")]
        public int Number { get; set; }

        [Display(Name = "Place Id")]
        public String PlaceId { get; set; }

        public Restaurant Restaurant { get; set; } // one to one (one Address to one Restaurant)
    }
}
