using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Mappers;
using Vinance.Contracts.Models.BaseModels;

namespace Vinance.Contracts.Extensions
{
    public static class MapperExtensions
    {
        public static IEnumerable<TOut> MapAll<TOut>(this IMapper mapper, IEnumerable<object> source)
        {
            return mapper.Map<IEnumerable<TOut>>(source);
        }
    }
}
