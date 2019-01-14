using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Enums;
    using Models;
    using Models.Helpers;

    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll(CategoryType? type, DateTime? from, DateTime? to);
        Task<IEnumerable<CategoryStatistics>> GetStats(CategoryType? type = null, DateTime? from = null, DateTime? to = null);
        Task<Category> Get(int categoryId);
        Task<Category> Create(Category category);
        Task<Category> Update(Category category);
        Task Delete(int categoryId);
    }
}