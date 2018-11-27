using Microsoft.Extensions.DependencyInjection;

namespace Vinance.Logic
{
    using Contracts.Interfaces;
    using Data;
    using Data.Contexts;
    using Services;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVinanceServices(this IServiceCollection services)
        {
            services.AddTransient<IFactory<VinanceContext>, VinanceContextFactory>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IIncomeService, IncomeService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            return services;
        }
    }
}