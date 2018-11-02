using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Exceptions.NotFound;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;
    using Identity.Interfaces;

    public class ExpenseService : IExpenseService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public ExpenseService(IIdentityService identityService, IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task<Expense> Create(Expense expense)
        {
            using (var context = _factory.Create())
            {
                var dataExpense = _mapper.Map<Data.Entities.Expense>(expense);
                dataExpense.UserId = _userId;
                context.Expenses.Add(dataExpense);
                await context.SaveChangesAsync();
                var addedExpense = await context.Expenses
                    .Include(e => e.From)
                    .Include(e => e.Category)
                    .SingleOrDefaultAsync(e => e.Id == dataExpense.Id);
                return _mapper.Map<Expense>(addedExpense);
            }
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataExpenses = await context.Expenses
                    .Where(e => e.UserId == _userId)
                    .Include(e => e.Category)
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
                    .Include(e => e.Category)
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
                context.Entry(dataExpense).Property(e => e.UserId).IsModified = false;
                await context.SaveChangesAsync();
                dataExpense = await context.Expenses
                    .Include(e => e.Category)
                    .Include(e => e.From)
                    .SingleOrDefaultAsync(e => e.Id == expense.Id);
                return _mapper.Map<Expense>(dataExpense);
            }
        }

        public async Task Delete(int expenseId)
        {
            using (var context = _factory.Create())
            {
                var dataExpense = await context.Expenses.FindAsync(expenseId);
                if (dataExpense == null)
                {
                    throw new ExpenseNotFoundException($"No expense found with id: {expenseId}");
                }

                context.Expenses.Remove(dataExpense);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Expense>> GetByAccountId(int accountId)
        {
            using (var context = _factory.Create())
            {
                var account = await context.Accounts
                    .Include(a => a.Expenses).ThenInclude(e => e.Category)
                    .SingleOrDefaultAsync(a => a.Id == accountId);
                if (account == null)
                {
                    throw new ExpenseNotFoundException($"No expense found with accountId: {accountId}");
                }

                return _mapper.MapAll<Expense>(account.Expenses.ToList());
            }
        }

        public async Task<IEnumerable<Expense>> GetByCategoryId(int categoryId)
        {
            using (var context = _factory.Create())
            {
                var expenses = await context.Expenses
                    .Where(e => e.CategoryId == categoryId)
                    .ToListAsync();
                return _mapper.MapAll<Expense>(expenses);
            }
        }
    }
}