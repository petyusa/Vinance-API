using System.Buffers;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Vinance.Api.ActionFilters;
using Vinance.Api.Middlewares;

namespace Vinance.Api
{
    using Contracts.Interfaces;
    using Data;
    using Data.Contexts;
    using Logic;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.OutputFormatters.Clear();
                opt.OutputFormatters.Add(
                    new JsonOutputFormatter(
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }, ArrayPool<char>.Shared
                        )
                    );
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<HeaderValidationFilterAttribute>();
            services.AddAutoMapper();
            services.AddVinanceServices();

            services.AddTransient<IFactory<VinanceContext>, VinanceContextFactory<VinanceContext>>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=payment}/{action=get}/{id?}");
            });

        }
    }
}
