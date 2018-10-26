using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models.BaseModels;

    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll<T>() where T : Category;
        Task<Category> Create(Category category);
    }
}