using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vinance.Contracts.Exceptions;

namespace Vinance.Logic.Services
{
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
                await context.Expenses.AddAsync(dataExpense);
                return _mapper.Map<Expense>(dataExpense);
            }
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataExpenses = await context.Expenses
                    .Include(p => p.From)
                    .Include(p => p.ExpenseCategory)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<Expense>>(dataExpenses);
            }
        }

        public async Task<Expense> GetById(int expenseId)
        {
            using (var context = _factory.Create())
            {
                var dataExpense = await context.Expenses
                    .Include(p => p.From)
                    .Include(p => p.ExpenseCategory)
                    .SingleOrDefaultAsync(p=>p.Id == expenseId);
                return _mapper.Map<Expense>(dataExpense);
            }
        }

        public async Task<Expense> Update(Expense expense)
        {
            using (var context = _factory.Create())
            {
                if (!context.Expenses.Any(p => p.Id == expense.Id))
                    return null;

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
                    return false;
                context.Expenses.Remove(dataExpense);
                return await context.SaveChangesAsync() == 1;
            }
        }
    }
}