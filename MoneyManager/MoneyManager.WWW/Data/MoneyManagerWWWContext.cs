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
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OutcomeCategory> OutcomeCategories { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }
    }
}
