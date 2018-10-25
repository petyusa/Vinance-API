using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vinance.Contracts.Extensions;
using Vinance.Contracts.Models.BaseModels;
using Vinance.Contracts.Models.Categories;
using Vinance.Identity;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;
    using Data.Contexts;

    public class CategoryService : ICategoryService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public CategoryService(IFactory<VinanceContext> factory, IIdentityService identityService, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task<IEnumerable<Category>> GetAll<T>() where T : Category
        {
            using (var context = _factory.Create())
            {
                IEnumerable<Data.Entities.Base.Category> categories;
                if (typeof(T) == typeof(ExpenseCategory))
                {
                    categories = await context.ExpenseCategories
                        .Include(ec => ec.Expenses)
                        .Where(ec => ec.UserId == _userId)
                        .ToListAsync();
                }
                else if (typeof(T) == typeof(IncomeCategory))
                {
                    categories = await context.IncomeCategories
                        .Include(ic => ic.Incomes)
                        .Where(ic => ic.UserId == _userId)
                        .ToListAsync();
                }
                else
                {
                    categories = await context.TransferCategories
                        .Include(tc => tc.Transfers)
                        .Where(ic => ic.UserId == _userId)
                        .ToListAsync();
                }
                return _mapper.MapAll<T>(categories);
            }
        }
    }
}