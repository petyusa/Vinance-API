using AutoMapper;

namespace Vinance.Api
{
    using Contracts.Models;
    using Viewmodels.Account;
    using Viewmodels.Category;
    using Viewmodels.Expense;
    using Viewmodels.Income;
    using Viewmodels.Transfer;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMissingTypeMaps = true;

            CreateMap<CreateAccountViewmodel, Account>()
                .ForMember(dest => dest.Balance, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Expenses, opt => opt.Ignore())
                .ForMember(dest => dest.Incomes, opt => opt.Ignore())
                .ForMember(dest => dest.TransfersTo, opt => opt.Ignore())
                .ForMember(dest => dest.TransfersFrom, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UpdateAccountViewmodel, Account>()
                .ForMember(dest => dest.Balance, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Expenses, opt => opt.Ignore())
                .ForMember(dest => dest.Incomes, opt => opt.Ignore())
                .ForMember(dest => dest.TransfersTo, opt => opt.Ignore())
                .ForMember(dest => dest.TransfersFrom, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CategoryViewmodel, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<CreateCategoryViewmodel, Category>()
                .ForMember(dest => dest.Balance, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UpdateCategoryViewmodel, Category>()
                .ForMember(dest => dest.Balance, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CreateExpenseViewmodel, Expense>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UpdateExpenseViewmodel, Expense>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CreateIncomeViewmodel, Income>()
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UpdateIncomeViewmodel, Income>()
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CreateTransferViewmodel, Transfer>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UpdateTransferViewmodel, Transfer>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

        }
    }
}