using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Enums;
    using Models;

    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll(CategoryType? type);
        Task<Category> Get(int categoryId);
        Task<Category> Create(Category category);
        Task<Category> Update(Category category);
        Task Delete(int categoryId);
    }
}