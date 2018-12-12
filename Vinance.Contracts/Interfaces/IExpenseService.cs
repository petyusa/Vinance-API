using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Enums;
    using Models;

    public interface IExpenseService
    {
        Task<Expense> Create(Expense expense);
        Task<IEnumerable<Expense>> Upload(IFormFile file);
        Task<IEnumerable<Expense>> GetAll(int? categoryId = null, DateTime? from = null, DateTime? to = null, string order = "date_desc");
        Task<Expense> GetById(int expenseId);
        Task<Expense> Update(Expense expense);
        Task Delete(int expenseId);
        Task<IEnumerable<Expense>> GetByAccountId(int accountId);
        Task<IEnumerable<Expense>> GetByCategoryId(int categoryId);
    }
}