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

    public class AccountService : IAccountService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public AccountService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<Account> Create(Account account)
        {
            using (var context = _factory.Create())
            {
                var dataAccount = _mapper.Map<Data.Entities.Account>(account);
                context.Accounts.Add(dataAccount);
                await context.SaveChangesAsync();
                return _mapper.Map<Account>(dataAccount);
            }
        }

        public async Task<Account> Get(int accountId)
        {
            using (var context = _factory.Create())
            {
                var dataAccount = await context.Accounts
                    .Include(a => a.Expenses)
                    .Include(a => a.Incomes)
                    .Include(a => a.TransfersFrom)
                    .Include(a => a.TransfersTo)
                    .SingleOrDefaultAsync(a => a.Id == accountId);

                if (dataAccount == null)
                {
                    throw new AccountNotFoundException($"No account found with id: {accountId}");
                }

                var account = _mapper.Map<Account>(dataAccount);
                return account;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataAccounts = await context.Accounts
                    .Include(a => a.Expenses)
                    .Include(a => a.Incomes)
                    .Include(a => a.TransfersFrom)
                    .Include(a => a.TransfersTo)
                    .ToListAsync();
                return _mapper.MapAll<Account>(dataAccounts);
            }
        }

        public async Task<Account> Update(Account account)
        {
            using (var context = _factory.Create())
            {
                if (!context.Accounts.Any(a => a.Id == account.Id))
                {
                    throw new AccountNotFoundException($"No account found with id: {account.Id}");
                }

                var dataAccount = _mapper.Map<Data.Entities.Account>(account);
                context.Entry(dataAccount).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return _mapper.Map<Account>(dataAccount);
            }
        }

        public async Task<bool> Delete(int accountId)
        {
            using (var context = _factory.Create())
            {
                var dataAccount = context.Accounts.Find(accountId);
                if (dataAccount == null)
                {
                    throw new AccountNotFoundException($"No account found with id: {accountId}");
                }

                context.Accounts.Remove(dataAccount);
                return await context.SaveChangesAsync() == 1;
            }
        }
    }
}
