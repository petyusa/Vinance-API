using AutoMapper;

namespace Vinance.Api
{
    using Contracts.Models;
    using Identity;
    using Viewmodels;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterViewmodel, VinanceUser>();
            CreateMap<Account, AccountViewmodel>();
            CreateMap<Expense, ExpenseViewmodel>();
            CreateMap<Income, IncomeViewmodel>();
            CreateMap<Transfer, TransferViewmodel>();
        }
    }
}