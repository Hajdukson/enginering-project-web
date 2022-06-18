using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;

namespace MoneyManager.DataAccess
{
    public class MoneyManagerDataContext : DbContext
    {
        public MoneyManagerDataContext (DbContextOptions<MoneyManagerDataContext> options)
            : base(options)
        {
           
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }
    }
}
