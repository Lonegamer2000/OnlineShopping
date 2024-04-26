using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using onlineshopping.Models;

namespace onlineshopping.Data
{
    public class onlineshoppingContext : DbContext
    {
        public onlineshoppingContext (DbContextOptions<onlineshoppingContext> options)
            : base(options)
        {
        }

        public DbSet<onlineshopping.Models.Category> Category { get; set; } = default!;
        public DbSet<onlineshopping.Models.Product> Product { get; set; } = default!;
        public DbSet<onlineshopping.Models.User> User { get; set; } = default!;
    }
}
