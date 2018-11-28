using AutoMapper;

namespace Vinance.Api
{
    using Contracts.Models;
    using Viewmodels;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMissingTypeMaps = true;
            CreateMap<CategoryViewmodel, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}