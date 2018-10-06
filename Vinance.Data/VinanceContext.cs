using Microsoft.EntityFrameworkCore;

namespace Vinance.Data
{
    public sealed class VinanceContext : DbContext
    {
        public VinanceContext(DbContextOptions options) : base (options)
        {
            Database.EnsureCreatedAsync();
        }
    }
}