using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
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
                var dataAccount = await context.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);
                return _mapper.Map<Account>(dataAccount);
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataAccounts = await context.Accounts.ToListAsync();
                return _mapper.Map<IEnumerable<Account>>(dataAccounts);
            }
        }

        public async Task<Account> Update(Account account)
        {
            using (var context = _factory.Create())
            {
                if (!context.Accounts.Any(a => a.Id == account.Id))
                    return null;

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
                    return false;
                context.Accounts.Remove(dataAccount);
                return await context.SaveChangesAsync() == 1;
            }
        }
    }
}
