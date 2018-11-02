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

        public async Task<IEnumerable<Category>> GetAll(CategoryType? type)
        {
            using (var context = _factory.Create())
            {
                var categories = context.Categories
                    .Where(ic => ic.UserId == _userId);

                if (type == null)
                {
                    return _mapper.MapAll<Category>(await categories.ToListAsync());
                }

                var dataType = _mapper.Map<Data.Enums.CategoryType>(type);
                var filteredCategories = await categories.Where(c => c.Type == dataType).ToListAsync();
                return _mapper.MapAll<Category>(filteredCategories);
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