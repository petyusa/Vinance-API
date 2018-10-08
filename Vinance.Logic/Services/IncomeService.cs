using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using System.Linq;
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
                var dataIncomes = await context.Incomes.Include(i => i.IncomeCategory).Include(i => i.To).ToListAsync();
                return _mapper.Map<IEnumerable<Income>>(dataIncomes);
            }
        }

        public async Task<Income> Create(Income income)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = _mapper.Map<Data.Entities.Income>(income);
                context.Incomes.Add(dataIncome);
                await context.SaveChangesAsync();
                return _mapper.Map<Income>(dataIncome);
            }
        }

        public async Task<Income> Get(int incomeId)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = await context.Incomes.Include(i=>i.IncomeCategory).Include(i=>i.To).SingleOrDefaultAsync(a => a.Id == incomeId);
                return _mapper.Map<Income>(dataIncome);
            }
        }

        public async Task<Income> Update(Income income)
        {
            using (var context = _factory.Create())
            {
                if (!context.Incomes.Any(a => a.Id == income.Id))
                    return null;

                var dataIncome = _mapper.Map<Data.Entities.Income>(income);
                context.Entry(dataIncome).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return _mapper.Map<Income>(dataIncome);
            }
        }

        public async Task<bool> Delete(int accountId)
        {
            using (var context = _factory.Create())
            {
                var dataIncome = context.Incomes.Find(accountId);
                if (dataIncome == null)
                    return false;
                context.Incomes.Remove(dataIncome);
                return await context.SaveChangesAsync() == 1;
            }
        }
    }
}