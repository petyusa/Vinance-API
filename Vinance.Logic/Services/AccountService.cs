using System.Collections.Generic;
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

        public async Task<IEnumerable<Account>> GetAll()
        {
            IEnumerable<Account> accounts;
            using (var context = _factory.Create())
            {
                var dataAccounts = await context.Accounts.ToListAsync();
                accounts = _mapper.Map<IEnumerable<Account>>(dataAccounts);
            }
            return accounts;
        }
    }
}
