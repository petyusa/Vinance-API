using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using System.Linq;
    using Contracts.Exceptions.NotFound;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;
    using Identity;

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

        public async Task<IEnumerable<Income>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataIncomes = await context.Incomes
                    .Include(i => i.Category)
                    .Include(i => i.To)
                    .Where(i => i.UserId == _userId)
                    .ToListAsync();
                return _mapper.MapAll<Income>(dataIncomes);
            }
        }

        public async Task<Income> Create(Income income)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = _mapper.Map<Data.Entities.Income>(income);
                dataIncome.UserId = _userId;
                context.Incomes.Add(dataIncome);
                await context.SaveChangesAsync();

                var createdIncome = _mapper.Map<Income>(dataIncome);
                return createdIncome;
            }

        }

        public async Task<Income> GetById(int incomeId)
        {
            using (var context = _factory.Create())
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
            using (var context = _factory.Create())
            {
                if (!context.Incomes.Any(i => i.Id == income.Id && i.UserId == _userId))
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
            using (var context = _factory.Create())
            {
                var dataIncome = context.Incomes.Find(incomeId);
                if (dataIncome == null || dataIncome.UserId != _userId)
                {
                    throw new IncomeNotFoundException($"No income found with id: {incomeId}");
                }

                context.Incomes.Remove(dataIncome);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Income>> GetByAccountId(int accountId)
        {
            using (var context = _factory.Create())
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
            using (var context = _factory.Create())
            {
                var incomes = await context.Incomes.Where(i => i.CategoryId == categoryId && i.UserId == _userId)
                    .ToListAsync();
                return _mapper.MapAll<Income>(incomes);
            }
        }
    }
}