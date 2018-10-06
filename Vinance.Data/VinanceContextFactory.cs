using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Vinance.Data
{
    using Contracts.Interfaces;

    public class VinanceContextFactory : IFactory<VinanceContext>
    {
        private readonly IConfiguration _config;

        public VinanceContextFactory(IConfiguration config)
        {
            _config = config;
        }

        public VinanceContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<VinanceContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("VinanceConnection"));

            return new VinanceContext(optionsBuilder.Options);
        }
    }
}