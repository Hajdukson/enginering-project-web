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
    }
}
