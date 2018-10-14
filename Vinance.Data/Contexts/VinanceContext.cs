using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Data.Contexts
{
    using Entities;
    using Entities.Categories;

    public sealed class VinanceContext : DbContext
    {
        public VinanceContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<TransferCategory> TransferCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Name = "Bankszámla"
                },
                new Account
                {
                    Id = 2,
                    Name = "Megtakarítás"
                }
            );

            modelBuilder.Entity<ExpenseCategory>().HasData(
                new ExpenseCategory
                {
                    Id = 1,
                    Name = "Extra kiadás"
                },
                new ExpenseCategory
                {
                    Id = 2,
                    Name = "Élelmiszer"
                }
            );

            modelBuilder.Entity<Expense>().HasData(
                new Expense
                {
                    Id = 1,
                    Amount = 4000,
                    Comment = "ez egy komment",
                    Date = DateTime.Now,
                    FromId = 1,
                    ExpenseCategoryId = 1

                },
                new Expense
                {
                    Id = 2,
                    Amount = 5000,
                    Comment = "ez egy másik komment",
                    Date = DateTime.Now,
                    FromId = 2,
                    ExpenseCategoryId = 2
                }
            );

            modelBuilder.Entity<IncomeCategory>().HasData(
                new IncomeCategory
                {
                    Id = 1,
                    Name = "Fizetés"
                },
                new IncomeCategory
                {
                    Id = 2,
                    Name = "Egyéb bevétel"
                }
            );

            modelBuilder.Entity<Income>().HasData(
                new Income
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Amount = 20000,
                    Comment = "this is an income comment",
                    IncomeCategoryId = 1,
                    ToId = 1
                },
                new Income
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Amount = 30000,
                    Comment = "this is another income comment",
                    IncomeCategoryId = 2,
                    ToId = 2
                }
            );

            modelBuilder.Entity<TransferCategory>().HasData(
                new TransferCategory
                {
                    Id = 1,
                    Name = "Kölcsönadás"
                },
                new TransferCategory
                {
                    Id = 2,
                    Name = "Levétel"
                }
            );

            modelBuilder.Entity<Transfer>().HasData(
                new Transfer
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Amount = 20000,
                    Comment = "this is a transfer comment",
                    FromId = 1,
                    ToId = 2,
                    TransferCategoryId = 1
                },
                new Transfer
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Amount = 20000,
                    Comment = "this is another transfer comment",
                    FromId = 2,
                    ToId = 1,
                    TransferCategoryId = 2
                }
            );

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
