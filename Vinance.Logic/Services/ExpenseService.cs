using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vinance.Logic.Services
{
    using Contracts.Enums;
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
            using (var context = _factory.CreateDbContext())
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

        public async Task<IEnumerable<Expense>> Upload(IFormFile file)
        {
            var expenses = new List<Data.Entities.Expense>();
            using (var stream = file.OpenReadStream())
            {
                var workbook = new XSSFWorkbook(stream);
                var sheet = workbook.GetSheet("Expenses");
                for (var rownum = 0; rownum <= sheet.LastRowNum; rownum++)
                {
                    if (sheet.GetRow(rownum) == null)
                        continue;

                    var row = sheet.GetRow(rownum);
                    var expense = new Data.Entities.Expense
                    {
                        Date = row.GetCell(0).DateCellValue,
                        FromId = (int)row.GetCell(1).NumericCellValue,
                        CategoryId = (int)row.GetCell(2).NumericCellValue,
                        Amount = (int)row.GetCell(3).NumericCellValue,
                        Comment = row.GetCell(4)?.StringCellValue,
                        UserId = _userId
                    };

                    expenses.Add(expense);
                }
            }

            using (var context = _factory.CreateDbContext())
            {
                await context.Expenses.AddRangeAsync(expenses);
                await context.SaveChangesAsync();
            }

            var mappedExpenses = _mapper.MapAll<Expense>(expenses);

            return mappedExpenses;
        }

        public async Task<IEnumerable<Expense>> GetAll(int? categoryId = null, DateTime? from = null, DateTime? to = null, string order = "date_desc")
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataExpenses = context.Expenses
                    .Where(e => e.UserId == _userId);

                if (from.HasValue && to.HasValue)
                {
                    dataExpenses = dataExpenses.Where(e => e.Date >= from.Value && e.Date <= to.Value);
                }

                if (categoryId.HasValue)
                {
                    dataExpenses = dataExpenses.Where(e => e.CategoryId == categoryId);
                }

                switch (order)
                {
                    case "date":
                        dataExpenses = dataExpenses.OrderBy(e => e.Date);
                        break;
                    case "date_desc":
                        dataExpenses = dataExpenses.OrderByDescending(e => e.Date);
                        break;
                    case "amount":
                        dataExpenses = dataExpenses.OrderBy(e => e.Amount);
                        break;
                    case "amount_desc":
                        dataExpenses = dataExpenses.OrderByDescending(e => e.Amount);
                        break;
                    default:
                        dataExpenses = dataExpenses.OrderByDescending(e => e.Date);
                        break;
                }

                var list = await dataExpenses
                    .Include(e => e.Category)
                    .Include(e => e.From)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Expense>>(list);
            }
        }

        public async Task<Expense> GetById(int expenseId)
        {
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
            {
                var expenses = await context.Expenses
                    .Where(e => e.CategoryId == categoryId)
                    .ToListAsync();
                return _mapper.MapAll<Expense>(expenses);
            }
        }
    }
}