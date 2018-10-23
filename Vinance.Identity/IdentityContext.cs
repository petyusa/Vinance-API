using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Identity
{
    public class IdentityContext : IdentityDbContext<VinanceUser>
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<VinanceUser> VinanceUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Identity");
            builder.Entity<VinanceUser>().ToTable("AspNetUsers");
            base.OnModelCreating(builder);
        }
    }
}