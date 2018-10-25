using AutoMapper;
using Vinance.Contracts.Models.Identity;

namespace Vinance.Api
{
    using Contracts.Models;
    using Identity;
    using Viewmodels;
    using Viewmodels.Identity;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterViewmodel, VinanceUser>();
            CreateMap<LoginViewmodel, LoginModel>();
            CreateMap<PasswordChangeViewmodel, PasswordChangeModel>();
            CreateMap<PasswordResetViewmodel, PasswordResetModel>();
            CreateMap<EmailChangeViewmodel, EmailChangeModel>();

            CreateMap<Account, AccountViewmodel>();
            CreateMap<Expense, ExpenseViewmodel>();
            CreateMap<Income, IncomeViewmodel>();
            CreateMap<Transfer, TransferViewmodel>();
        }
    }
}