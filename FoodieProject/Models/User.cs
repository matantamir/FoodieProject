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

        [Required, Display(Name = "Username")]
        public string Username { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "User Role")]
        public UserRole Type { get; set; }

    }
}
