using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Vinance.Identity
{
    using Contracts.Interfaces;

    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>, IFactory<IdentityContext>
    {
        private readonly IConfiguration _config;

        public IdentityContextFactory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public IdentityContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public IdentityContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("IdentityConnection"));

            var context = new IdentityContext(optionsBuilder.Options);
            return context;
        }
    }
}