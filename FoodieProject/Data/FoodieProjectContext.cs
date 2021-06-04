using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodieProject.Models;

namespace FoodieProject.Data
{
    public class FoodieProjectContext : DbContext
    {
        public FoodieProjectContext (DbContextOptions<FoodieProjectContext> options)
            : base(options)
        {
        }

        public DbSet<FoodieProject.Models.About> About { get; set; }

        public DbSet<FoodieProject.Models.Address> Address { get; set; }

        public DbSet<FoodieProject.Models.Dish> Dish { get; set; }

        public DbSet<FoodieProject.Models.DishTag> DishTag { get; set; }

        public DbSet<FoodieProject.Models.Restaurant> Restaurant { get; set; }

        public DbSet<FoodieProject.Models.Tag> Tag { get; set; }

        public DbSet<FoodieProject.Models.User> User { get; set; }
    }
}
