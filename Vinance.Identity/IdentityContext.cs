using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Identity
{
    using Entities;

    public class IdentityContext : IdentityDbContext<VinanceUser, IdentityRole<Guid>, Guid>
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<VinanceUser> VinanceUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Identity");
            builder.Entity<VinanceUser>().ToTable("AspNetUsers");
            builder.Entity<RefreshToken>().ToTable("RefreshTokens");
            base.OnModelCreating(builder);
        }
    }
}