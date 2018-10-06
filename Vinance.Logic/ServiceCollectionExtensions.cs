using Microsoft.Extensions.DependencyInjection;

namespace Vinance.Logic
{
    using Contracts.Interfaces;
    using Services;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVinanceServices(this IServiceCollection services)
        {
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IIncomeService, IncomeService>();
            return services;
        }
    }
}