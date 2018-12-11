using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Vinance.Identity
{
    using Contracts.Interfaces;
    using Entities;
    using Interfaces;
    using Services;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVinanceIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["TokenAuthentication:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["TokenAuthentication:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]))
                };
            });

            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddTransient<IFactory<IdentityContext>, IdentityContextFactory>();
            services.AddTransient<ITokenHandler, TokenHandler>();
            services.AddTransient<IIdentityService, IdentityService>();

            var builder = services.AddIdentityCore<VinanceUser>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole<Guid>), builder.Services);
            builder.AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            builder.AddRoleManager<RoleManager<IdentityRole<Guid>>>();
            return services;
        }
    }

}