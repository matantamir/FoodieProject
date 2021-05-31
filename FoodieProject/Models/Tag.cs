﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieProject.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Name { get; set; }

        public List<Restaurant> Restaurants { get; set; }  // many to many (many Tags to many Restaurant)

    }
}
