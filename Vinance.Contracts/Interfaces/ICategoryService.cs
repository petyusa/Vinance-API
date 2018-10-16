using System.Collections.Generic;
using System.Threading.Tasks;
using Vinance.Contracts.Models.BaseModels;

namespace Vinance.Contracts.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll<T>() where T : Category;
    }
}