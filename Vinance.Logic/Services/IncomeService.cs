using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;

namespace Vinance.Logic.Services
{
    using Contracts.Exceptions.NotFound;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;
    using Identity.Interfaces;

    public class IncomeService : IIncomeService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public IncomeService(IFactory<VinanceContext> factory, IIdentityService identityService, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task<IEnumerable<Income>> GetAll(int? categoryId = null, DateTime? from = null, DateTime? to = null, string order = "date_desc")
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataIncomes = context.Incomes.Where(i => i.UserId == _userId);

                
                    if (from.HasValue && to.HasValue)
                    {
                        dataIncomes = dataIncomes.Where(e => e.Date >= from.Value && e.Date <= to.Value);
                    }

                    if (categoryId.HasValue)
                    {
                        dataIncomes = dataIncomes.Where(e => e.CategoryId == categoryId);
                    }

                    switch (order)
                    {
                        case "date":
                            dataIncomes = dataIncomes.OrderBy(e => e.Date);
                            break;
                        case "date_desc":
                            dataIncomes = dataIncomes.OrderByDescending(e => e.Date);
                            break;
                        case "amount":
                            dataIncomes = dataIncomes.OrderBy(e => e.Amount);
                            break;
                        case "amount_desc":
                            dataIncomes = dataIncomes.OrderByDescending(e => e.Amount);
                            break;
                        default:
                            dataIncomes = dataIncomes.OrderByDescending(e => e.Date);
                            break;
                    }

                    var list = await dataIncomes
                        .Include(i => i.Category)
                        .Include(i => i.To)
                        .ToListAsync();

                return _mapper.MapAll<Income>(list);
            }
        }

        public async Task<Income> Create(Income income)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataIncome = _mapper.Map<Data.Entities.Income>(income);
                dataIncome.UserId = _userId;
                context.Incomes.Add(dataIncome);
                await context.SaveChangesAsync();

                var createdIncome = _mapper.Map<Income>(dataIncome);
                return createdIncome;
            }

        }

        public async Task<IEnumerable<Income>> Upload(StreamReader stream)
        {
            var incomes = new List<Data.Entities.Income>();
            using (stream)
            {
                var workbook = new XSSFWorkbook(stream.BaseStream);
                var sheet = workbook.GetSheet("Incomes");
                for (var rownum = 0; rownum <= sheet.LastRowNum; rownum++)
                {
                    if (sheet.GetRow(rownum) == null)
                    {
                        continue;
                    }

                    var row = sheet.GetRow(rownum);
                    var income = new Data.Entities.Income
                    {
                        Date = row.GetCell(0).DateCellValue,
                        ToId = (int)row.GetCell(1).NumericCellValue,
                        CategoryId = (int)row.GetCell(2).NumericCellValue,
                        Amount = (int)row.GetCell(3).NumericCellValue,
                        Comment = row.GetCell(4)?.StringCellValue,
                        UserId = _userId
                    };

                    incomes.Add(income);
                }
            }

            using (var context = _factory.CreateDbContext())
            {
                await context.Incomes.AddRangeAsync(incomes);
                await context.SaveChangesAsync();
            }

            var mappedExpenses = _mapper.MapAll<Income>(incomes);

            return mappedExpenses;
        }

        public async Task<Income> GetById(int incomeId)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataIncome = await context.Incomes
                    .Include(i => i.To)
                    .Include(i => i.Category)
                    .SingleOrDefaultAsync(i => i.Id == incomeId && i.UserId == _userId);

                if (dataIncome == null)
                {
                    throw new IncomeNotFoundException($"No income found with id: {incomeId}");
                }
                return _mapper.Map<Income>(dataIncome);
            }
        }

        public async Task<Income> Update(Income income)
        {
            using (var context = _factory.CreateDbContext())
            {
                if (!context.Incomes.Any(i => i.Id == income.Id))
                {
                    throw new IncomeNotFoundException($"No income found with id: {income.Id}");
                }

                var dataIncome = _mapper.Map<Data.Entities.Income>(income);
                context.Entry(dataIncome).State = EntityState.Modified;
                context.Entry(dataIncome).Property(i => i.UserId).IsModified = false;
                await context.SaveChangesAsync();
                dataIncome = context.Incomes.Find(income.Id);
                return _mapper.Map<Income>(dataIncome);
            }
        }

        public async Task Delete(int incomeId)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataIncome = context.Incomes.Find(incomeId);
                if (dataIncome == null)
                {
                    throw new IncomeNotFoundException($"No income found with id: {incomeId}");
                }

                context.Incomes.Remove(dataIncome);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Income>> GetByAccountId(int accountId)
        {
            using (var context = _factory.CreateDbContext())
            {
                var account = await context.Accounts.Include(a => a.Incomes)
                    .SingleOrDefaultAsync(a => a.Id == accountId && a.UserId == _userId);
                if (account == null)
                {
                    throw new IncomeNotFoundException($"No income found with accountId: {accountId}");
                }
                return _mapper.MapAll<Income>(account.Incomes.ToList());
            }
        }

        public async Task<IEnumerable<Income>> GetByCategoryId(int categoryId)
        {
            using (var context = _factory.CreateDbContext())
            {
                var incomes = await context.Incomes.Where(i => i.CategoryId == categoryId && i.UserId == _userId)
                    .ToListAsync();
                return _mapper.MapAll<Income>(incomes);
            }
        }
    }
}