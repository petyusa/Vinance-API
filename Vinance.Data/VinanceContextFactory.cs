using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Vinance.Data
{
    using Contracts.Interfaces;

    public class VinanceContextFactory<TContext> : IFactory<TContext> where TContext : DbContext
    {
        private readonly IConfiguration _config;

        public VinanceContextFactory(IConfiguration config)
        {
            _config = config;
        }

        public TContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("VinanceConnection"));

            var context = (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
            return context;
        }
    }
}