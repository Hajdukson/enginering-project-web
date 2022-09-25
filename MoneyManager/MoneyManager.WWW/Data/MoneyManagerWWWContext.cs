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
        public DbSet<SingleOutcomeCategory> SingleOutcomeCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Item> Outcomes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }
    }
}
