using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vinance.Contracts.Extensions;
using Vinance.Contracts.Models.BaseModels;
using Vinance.Contracts.Models.Categories;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;
    using Data.Contexts;

    public class CategoryService : ICategoryService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public CategoryService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAll<T>() where T : Category
        {
            using (var context = _factory.Create())
            {
                var categories = await context.Set<T>().ToListAsync();
                return _mapper.MapAll<T>(categories);
            }
        }
    }
}