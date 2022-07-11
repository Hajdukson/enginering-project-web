using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyManager.WWW.Models;

namespace MoneyManager.WWW.Data
{
    public class MoneyManagerWWWContext : DbContext
    {
        public MoneyManagerWWWContext (DbContextOptions<MoneyManagerWWWContext> options)
            : base(options)
        {
        }

        public DbSet<MoneyManager.WWW.Models.Category>? Category { get; set; }
    }
}
