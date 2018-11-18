using AutoMapper;

namespace Vinance.Api
{
    using Contracts.Models;
    using Contracts.Models.Identity;
    using Viewmodels;
    using Viewmodels.Identity;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMissingTypeMaps = true;
            CreateMap<RegisterViewmodel, RegisterModel>();
            CreateMap<LoginViewmodel, LoginModel>();
            CreateMap<PasswordChangeViewmodel, PasswordChangeModel>();
            CreateMap<PasswordResetViewmodel, PasswordResetModel>();
            CreateMap<EmailChangeViewmodel, EmailChangeModel>();

            CreateMap<Account, AccountViewmodel>();
            CreateMap<Expense, ExpenseViewmodel>();
            CreateMap<Income, IncomeViewmodel>();
            CreateMap<Transfer, TransferViewmodel>();
            CreateMap<Category, CategoryViewmodel>();
        }
    }
}