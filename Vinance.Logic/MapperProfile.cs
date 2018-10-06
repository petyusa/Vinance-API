using AutoMapper;

namespace Vinance.Logic
{
    using Contracts.Models;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Data.Entities.Payment, Payment>();
        }
    }
}