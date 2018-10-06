using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
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
            IEnumerable<Income> incomes;
            using (var context = _factory.Create())
            {
                var dataIncomes = await context.Incomes.Include(i => i.IncomeCategory).Include(i => i.To).ToListAsync();
                incomes = _mapper.Map<IEnumerable<Income>>(dataIncomes);
            }
            return incomes;
        }
    }
}