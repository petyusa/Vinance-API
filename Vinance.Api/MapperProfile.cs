using AutoMapper;

namespace Vinance.Api
{
    using Contracts.Models;
    using Viewmodels;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Account, AccountViewmodel>();
            CreateMap<Expense, ExpenseViewmodel>();
            CreateMap<Income, IncomeViewmodel>();
            CreateMap<Transfer, TransferViewmodel>();
        }
    }
}