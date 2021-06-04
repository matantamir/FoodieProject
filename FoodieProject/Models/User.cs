using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public enum UserType
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

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserType Type { get; set; }

    }
}
