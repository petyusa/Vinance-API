using AutoMapper;

namespace Vinance.Logic
{
    using Contracts.Models;
    using Contracts.Models.Categories;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Data.Entities.Account, Account>().ReverseMap();
            CreateMap<Data.Entities.Expense, Expense>().ReverseMap();
            CreateMap<Data.Entities.Income, Income>().ReverseMap();
            CreateMap<Data.Entities.Transfer, Transfer>().ReverseMap();
            CreateMap<Data.Entities.Categories.ExpenseCategory, ExpenseCategory>().ReverseMap();
            CreateMap<Data.Entities.Categories.IncomeCategory, IncomeCategory>().ReverseMap();
            CreateMap<Data.Entities.Categories.TransferCategory, TransferCategory>().ReverseMap();
        }
    }
}