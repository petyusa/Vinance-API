using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vinance.Contracts.Models.BaseModels;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data;

    public class TransactionService : ITransactionService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public TransactionService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            IEnumerable<Account> accounts;
            using (var context = _factory.Create())
            {
                var dataAccounts = await context.Accounts.ToListAsync();
                accounts = _mapper.Map<IEnumerable<Account>>(dataAccounts);
            }
            return accounts;
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            IEnumerable<Payment> payments;
            using (var context = _factory.Create())
            {
                var dataPayments = await context.Payments
                    .Include(p => p.From)
                    .Include(p => p.PaymentCategory)
                    .ToListAsync();
                payments = _mapper.Map<IEnumerable<Payment>>(dataPayments);
            }
            return payments;
        }

        public async Task<IEnumerable<Income>> GetIncomes()
        {
            IEnumerable<Income> incomes;
            using (var context = _factory.Create())
            {
                var dataIncomes = await context.Incomes
                    .Include(i => i.To)
                    .Include(i => i.IncomeCategory)
                    .ToListAsync();
                incomes = _mapper.Map<IEnumerable<Income>>(dataIncomes);
            }
            return incomes;
        }

        public async Task<IEnumerable<Transfer>> GetTransfers()
        {
            IEnumerable<Transfer> transfers;
            using (var context = _factory.Create())
            {
                var dataTransfers = await context.Transfers
                    .Include(t => t.From)
                    .Include(t => t.To)
                    .Include(t => t.TransferCategory)
                    .ToListAsync();
                transfers = _mapper.Map<IEnumerable<Transfer>>(dataTransfers);
            }
            return transfers;
        }

        public async Task<IEnumerable<T>> GetCategory<T>() where T : Category
        {
            IEnumerable<T> categories;
            using (var context = _factory.Create())
            {
                var dataCategories = await context.Set<T>().ToListAsync();
                categories = _mapper.Map<IEnumerable<T>>(dataCategories);
            }
            return categories;
        }
    }
}