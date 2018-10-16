using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using System.Linq;
    using Contracts.Exceptions;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;

    public class IncomeService : IIncomeService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public IncomeService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Income>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataIncomes = await context.Incomes
                    .Include(i => i.IncomeCategory)
                    .Include(i => i.To)
                    .ToListAsync();
                return _mapper.MapAll<Income>(dataIncomes);
            }
        }

        public async Task<Income> Create(Income income)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = _mapper.Map<Data.Entities.Income>(income);
                context.Incomes.Add(dataIncome);
                await context.SaveChangesAsync();

                var createdIncome = _mapper.Map<Income>(dataIncome);
                return createdIncome;
            }

        }

        public async Task<Income> Get(int incomeId)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = await context.Incomes
                    .Include(i => i.To)
                    .Include(i => i.IncomeCategory)
                    .SingleOrDefaultAsync(a => a.Id == incomeId);

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
                if (!context.Incomes.Any(i => i.Id == income.Id))
                {
                    throw new IncomeNotFoundException($"No income found with id: {income.Id}");
                }

                var updatedIncome = _mapper.Map<Data.Entities.Income>(income);
                context.Entry(updatedIncome).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return _mapper.Map<Income>(updatedIncome);
            }
        }

        public async Task<bool> Delete(int incomeId)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = context.Incomes.Find(incomeId);
                if (dataIncome == null)
                {
                    throw new IncomeNotFoundException($"No income found with id: {incomeId}");
                }

                context.Incomes.Remove(dataIncome);
                return await context.SaveChangesAsync() == 1;
            }
        }

        public async Task<IEnumerable<Income>> GetByAccountId(int accountId)
        {
            using (var context = _factory.Create())
            {
                var incomes = await context.Incomes.Where(i => i.ToId == accountId).ToListAsync();
                return _mapper.MapAll<Income>(incomes);
            }
        }

        public async Task<IEnumerable<Income>> GetByCategoryId(int categoryId)
        {
            using (var context = _factory.Create())
            {
                var incomes = await context.Incomes.Where(i => i.IncomeCategoryId == categoryId).ToListAsync();
                return _mapper.MapAll<Income>(incomes);
            }
        }
    }
}