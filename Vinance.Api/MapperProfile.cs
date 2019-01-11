using AutoMapper;

namespace Vinance.Api
{
    using Contracts.Models;
    using Viewmodels;
    using Viewmodels.Account;

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
                .ForMember(dest => dest.TransfersFrom, opt => opt.Ignore());

            CreateMap<UpdateAccountViewmodel, Account>()
                .ForMember(dest => dest.Balance, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Expenses, opt => opt.Ignore())
                .ForMember(dest => dest.Incomes, opt => opt.Ignore())
                .ForMember(dest => dest.TransfersTo, opt => opt.Ignore())
                .ForMember(dest => dest.TransfersFrom, opt => opt.Ignore());

            CreateMap<CategoryViewmodel, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}