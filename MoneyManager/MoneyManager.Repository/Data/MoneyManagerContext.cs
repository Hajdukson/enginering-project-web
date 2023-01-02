using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;

namespace MoneyManager.Repository
{
    public class MoneyManagerContext : IdentityDbContext
    {
        public MoneyManagerContext (DbContextOptions<MoneyManagerContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoughtProduct>().HasData(
                new BoughtProduct { Id = 1, BoughtDate = new DateTime(2022, 12, 01), Price = 2.00m, Name = "Snickers" },
                new BoughtProduct { Id = 2, BoughtDate = new DateTime(2022, 12, 05), Price = 2.03m, Name = "Snickers" },
                new BoughtProduct { Id = 3, BoughtDate = new DateTime(2022, 12, 08), Price = 2.04m, Name = "Snickers" },
                new BoughtProduct { Id = 4, BoughtDate = new DateTime(2022, 12, 15), Price = 2.10m, Name = "Snickers" },
                new BoughtProduct { Id = 5, BoughtDate = new DateTime(2022, 12, 20), Price = 2.12m, Name = "Snickers" },
                new BoughtProduct { Id = 6, BoughtDate = new DateTime(2022, 12, 23), Price = 2.17m, Name = "Snickers" },
                new BoughtProduct { Id = 7, BoughtDate = new DateTime(2022, 12, 30), Price = 2.25m, Name = "Snickers" },
                new BoughtProduct { Id = 8, BoughtDate = new DateTime(2023, 01, 01), Price = 2.23m, Name = "Snickers" },
                new BoughtProduct { Id = 9, BoughtDate = new DateTime(2022, 12, 01), Price = 0.30m, Name = "Kajzerka" },
                new BoughtProduct { Id = 11, BoughtDate = new DateTime(2022, 12, 05), Price = 0.32m, Name = "Kajzerka" },
                new BoughtProduct { Id = 12, BoughtDate = new DateTime(2022, 12, 08), Price = 0.40m, Name = "Kajzerka" },
                new BoughtProduct { Id = 13, BoughtDate = new DateTime(2022, 12, 15), Price = 0.46m, Name = "Kajzerka" },
                new BoughtProduct { Id = 14, BoughtDate = new DateTime(2022, 12, 20), Price = 0.50m, Name = "Kajzerka" },
                new BoughtProduct { Id = 15, BoughtDate = new DateTime(2022, 12, 23), Price = 0.54m, Name = "Kajzerka" },
                new BoughtProduct { Id = 16, BoughtDate = new DateTime(2022, 12, 30), Price = 0.60m, Name = "Kajzerka" },
                new BoughtProduct { Id = 17, BoughtDate = new DateTime(2023, 01, 01), Price = 0.62m, Name = "Kajzerka" },
                new BoughtProduct { Id = 18, BoughtDate = new DateTime(2022, 12, 01), Price = 2.00m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 19, BoughtDate = new DateTime(2022, 12, 05), Price = 2.07m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 20, BoughtDate = new DateTime(2022, 12, 08), Price = 2.10m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 21, BoughtDate = new DateTime(2022, 12, 15), Price = 2.20m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 22, BoughtDate = new DateTime(2022, 12, 20), Price = 2.24m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 23, BoughtDate = new DateTime(2022, 12, 23), Price = 2.28m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 24, BoughtDate = new DateTime(2022, 12, 30), Price = 2.30m, Name = "1 kg Jabłek" },
                new BoughtProduct { Id = 25, BoughtDate = new DateTime(2023, 01, 01), Price = 2.31m, Name = "1 kg Jabłek" }
            );
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OutcomeCategory> OutcomeCategories { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BoughtProduct> BoughtProducts { get; set; }
    }
}
