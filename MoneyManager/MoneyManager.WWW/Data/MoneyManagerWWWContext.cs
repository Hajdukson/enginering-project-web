using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;

namespace MoneyManager.WWW.Data
{
    public class MoneyManagerWWWContext : DbContext
    {
        public MoneyManagerWWWContext (DbContextOptions<MoneyManagerWWWContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }
    }
}
