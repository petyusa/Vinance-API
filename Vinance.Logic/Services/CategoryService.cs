using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Enums;
    using Contracts.Exceptions.NotFound;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;
    using Identity.Interfaces;

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

        public async Task<IEnumerable<Category>> GetAll(CategoryType? type, DateTime? from, DateTime? to)
        {
            using (var context = _factory.Create())
            {
                var categories = context.Categories
                    .Where(ic => ic.UserId == _userId);


                IEnumerable<Category> mappedCategories;
                if (type == null)
                {
                    mappedCategories = _mapper.MapAll<Category>(await categories.ToListAsync()).ToList();
                }
                else
                {
                    var dataType = _mapper.Map<Data.Enums.CategoryType>(type);
                    mappedCategories = _mapper.MapAll<Category>(await categories.Where(c => c.Type == dataType).ToListAsync()).ToList();
                }

                if (from.HasValue && to.HasValue)
                {
                    foreach (var category in mappedCategories)
                    {
                        switch (category.Type)
                        {
                            case CategoryType.Expense:
                                category.Balance = context.Expenses
                                    .Where(e => e.UserId == _userId && e.CategoryId == category.Id && e.Date >= from.Value && e.Date <= to.Value).Sum(e => e.Amount);
                                break;
                            case CategoryType.Income:
                                category.Balance = context.Incomes
                                    .Where(i => i.UserId == _userId && i.CategoryId == category.Id && i.Date >= from.Value && i.Date <= to.Value).Sum(i => i.Amount);
                                break;
                            case CategoryType.Transfer:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                else
                {
                    foreach (var category in mappedCategories)
                    {
                        switch (category.Type)
                        {
                            case CategoryType.Expense:
                                category.Balance = context.Expenses
                                    .Where(e => e.UserId == _userId && e.CategoryId == category.Id).Sum(e => e.Amount);
                                break;
                            case CategoryType.Income:
                                category.Balance = context.Incomes
                                    .Where(i => i.UserId == _userId && i.CategoryId == category.Id).Sum(i => i.Amount);
                                break;
                            case CategoryType.Transfer:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                }

                return mappedCategories;
            }
        }

        public async Task<Category> Get(int categoryId)
        {
            using (var context = _factory.Create())
            {
                var category = await context.Categories
                    .SingleOrDefaultAsync(c => c.Id == categoryId && c.UserId == _userId);
                if (category == null)
                {
                    throw new CategoryNotFoundException($"No category found with id: {categoryId}");
                }

                return _mapper.Map<Category>(category);
            }
        }

        public async Task<Category> Create(Category category)
        {
            using (var context = _factory.Create())
            {
                var dataCategory = _mapper.Map<Data.Entities.Category>(category);
                dataCategory.UserId = _userId;
                context.Categories.Add(dataCategory);
                await context.SaveChangesAsync();
                return _mapper.Map<Category>(dataCategory);
            }
        }

        public async Task<Category> Update(Category category)
        {
            using (var context = _factory.Create())
            {
                if (!context.Categories.Any(c => c.Id == category.Id && c.UserId == _userId))
                {
                    throw new CategoryNotFoundException($"No category found with id: {category.Id}");
                }

                var dataCategory = _mapper.Map<Data.Entities.Category>(category);
                context.Entry(dataCategory).State = EntityState.Modified;
                context.Entry(dataCategory).Property(c => c.UserId).IsModified = false;
                await context.SaveChangesAsync();
                dataCategory = await context.Categories.FindAsync(category.Id);
                return _mapper.Map<Category>(dataCategory);
            }
        }

        public async Task Delete(int categoryId)
        {
            using (var context = _factory.Create())
            {
                var category = await context.Categories
                    .SingleOrDefaultAsync(c => c.Id == categoryId && c.UserId == _userId);
                if (category == null || category.UserId != _userId)
                {
                    throw new CategoryNotFoundException($"No category found with id: {categoryId}");
                }

                context.Remove(category);
                await context.SaveChangesAsync();
            }
        }
    }
}