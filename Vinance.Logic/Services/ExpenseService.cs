using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Exceptions;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;

    public class ExpenseService : IExpenseService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public ExpenseService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<Expense> Create(Expense expense)
        {
            using (var context = _factory.Create())
            {
                var dataExpense = _mapper.Map<Data.Entities.Expense>(expense);
                context.Expenses.Add(dataExpense);
                await context.SaveChangesAsync();
                return _mapper.Map<Expense>(dataExpense);
            }
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataExpenses = await context.Expenses
                    .Include(e => e.ExpenseCategory)
                    .Include(e => e.From)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<Expense>>(dataExpenses);
            }
        }

        public async Task<Expense> GetById(int expenseId)
        {
            using (var context = _factory.Create())
            {
                var dataExpense = await context.Expenses
                    .Include(e => e.From)
                    .Include(e => e.ExpenseCategory)
                    .SingleOrDefaultAsync(e => e.Id == expenseId);

                if (dataExpense == null)
                {
                    throw new ExpenseNotFoundException($"No expense found with id: {expenseId}");
                }

                return _mapper.Map<Expense>(dataExpense);
            }
        }

        public async Task<Expense> Update(Expense expense)
        {
            using (var context = _factory.Create())
            {
                if (!context.Expenses.Any(e => e.Id == expense.Id))
                {
                    throw new ExpenseNotFoundException($"No expense found with id: {expense.Id}");
                }

                var dataExpense = _mapper.Map<Data.Entities.Expense>(expense);
                context.Entry(dataExpense).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return _mapper.Map<Expense>(dataExpense);
            }
        }

        public async Task<bool> Delete(int expenseId)
        {
            using (var context = _factory.Create())
            {
                var dataExpense = await context.Expenses.FindAsync(expenseId);
                if (dataExpense == null)
                {
                    throw new ExpenseNotFoundException($"No expense found with id: {expenseId}");
                }

                context.Expenses.Remove(dataExpense);
                return await context.SaveChangesAsync() == 1;
            }
        }

        public async Task<IEnumerable<Expense>> GetByAccountId(int accountId)
        {
            using (var context = _factory.Create())
            {
                var expenses = await context.Expenses.Where(e => e.FromId == accountId).ToListAsync();
                return _mapper.MapAll<Expense>(expenses);
            }
        }

        public async Task<IEnumerable<Expense>> GetByCategoryId(int categoryId)
        {
            using (var context = _factory.Create())
            {
                var expenses = await context.Expenses.Where(e => e.ExpenseCategoryId == categoryId).ToListAsync();
                return _mapper.MapAll<Expense>(expenses);
            }
        }
    }
}