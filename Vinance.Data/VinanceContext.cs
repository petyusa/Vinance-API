using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Data
{
    using Entities;
    using Entities.Categories;

    public sealed class VinanceContext : DbContext
    {
        public VinanceContext(DbContextOptions options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<PaymentCategory> PaymentCategories { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<TransferCategory> TransferCategories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(new Account { Balance = 1000, Name = "Petyka pénztárca", Id = 1 });

            modelBuilder.Entity<PaymentCategory>().HasData(new PaymentCategory { Id = 1, Name = "Extra kiadás" });

            modelBuilder.Entity<IncomeCategory>();

            modelBuilder.Entity<TransferCategory>();

            modelBuilder.Entity<Payment>().HasData(
                new Payment { Amount = 123, Comment = "mycomment", Date = DateTime.Today, FromId = 1, Id = 1, PaymentCategoryId = 1 },
                new Payment { Amount = 456, Comment = "mycomment", Date = DateTime.Today, FromId = 1, Id = 2, PaymentCategoryId = 1 }
                );

            modelBuilder.Entity<Income>();

            modelBuilder.Entity<Transfer>();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
