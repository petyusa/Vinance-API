using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Microsoft.AspNetCore.Http;
    using Models;

    public interface IExpenseService
    {
        Task<Expense> Create(Expense expense);
        Task<IEnumerable<Expense>> Upload(IFormFile file);
        Task<IEnumerable<Expense>> GetAll();
        Task<Expense> GetById(int expenseId);
        Task<Expense> Update(Expense expense);
        Task Delete(int expenseId);
        Task<IEnumerable<Expense>> GetByAccountId(int accountId);
        Task<IEnumerable<Expense>> GetByCategoryId(int categoryId);
    }
}