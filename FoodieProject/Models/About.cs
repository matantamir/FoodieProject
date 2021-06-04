using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class About
    {

        [Key]
        public int Id { get; set; }

        [Required,Display(Name = "Restaurant About")]
        public string Name { get; set; }

        [Display(Name = "About Description"), Required(ErrorMessage = "About cannot be submitted witout a description")]
        public string Description { get; set; }
        
        [Required, Display(Name = "Author")]
        public string Author { get; set; }
        
        [Display(Name = "Last Update Date")]
        public DateTime LastUpdateDate { get; set; }

        [Required]
        public Restaurant Restaurant { get; set; } // one to one (one About to one Restaurant)

    }
}
