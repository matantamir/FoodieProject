using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public enum UserRole
    {
        Admin,
        Client
    }
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "User Role")]
        public UserRole Type { get; set; }

    }
}
