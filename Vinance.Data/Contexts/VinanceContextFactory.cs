using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Vinance.Data.Contexts
{
    using Contracts.Interfaces;

    public class VinanceContextFactory : IDesignTimeDbContextFactory<VinanceContext>, IFactory<VinanceContext>
    {
        private readonly IConfiguration _config;

        public VinanceContextFactory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public VinanceContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public VinanceContext CreateDbContext()
        {
            var connString = _config.GetConnectionString("VinanceConnection");
            var optionsBuilder = new DbContextOptionsBuilder<VinanceContext>();
            optionsBuilder.UseSqlServer(connString);

            var context = new VinanceContext(optionsBuilder.Options);
            return context;
        }
    }
}