using AutoMapper;
using Vinance.Api.Viewmodels;
using Vinance.Contracts.Models;

namespace Vinance.Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Account, AccountViewmodel>();
            CreateMap<Payment, PaymentViewmodel>();
        }
    }
}