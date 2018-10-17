using System;
using System.Collections;
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
                IEnumerable<Data.Entities.Base.Category> categories;
                if (typeof(T) == typeof(ExpenseCategory))
                {
                    categories = await context.ExpenseCategories.Include(ec => ec.Expenses).ToListAsync();
                }
                else if (typeof(T) == typeof(IncomeCategory))
                {
                    categories = await context.IncomeCategories.Include(ic => ic.Incomes).ToListAsync();
                }
                else
                {
                    categories = await context.TransferCategories.Include(tc => tc.Transfers).ToListAsync();
                }
                return _mapper.MapAll<T>(categories);
            }
        }
    }
}