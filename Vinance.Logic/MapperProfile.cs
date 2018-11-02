using AutoMapper;

namespace Vinance.Logic
{
    using Contracts.Models;
    using Contracts.Models.Identity;
    using Identity.Entities;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterModel, VinanceUser>();

            CreateMap<Data.Entities.Account, Account>().ReverseMap();
            CreateMap<Data.Entities.Expense, Expense>().ReverseMap();
            CreateMap<Data.Entities.Income, Income>().ReverseMap();
            CreateMap<Data.Entities.Transfer, Transfer>().ReverseMap();
            CreateMap<Data.Entities.Category, Category>().ReverseMap();
        }
    }
}