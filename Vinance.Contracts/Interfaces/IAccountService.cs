using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;
    using Models.Helpers;

    public interface IAccountService
    {
        Task<Account> Create(Account account);
        Task<Account> Get(int accountId);
        Task<IEnumerable<Account>> GetAll();
        Task<Account> Update(Account account);
        Task Delete(int account);
        List<DailyBalanceList> GetDailyBalances(DateTime? from = null, DateTime? to = null);
    }
}