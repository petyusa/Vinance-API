using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Enums;
    using Models;
    using Models.Helpers;

    public interface IAccountService
    {
        Task<Account> Create(Account account);
        Task<Account> Get(int accountId);
        Task<IEnumerable<Account>> GetAll(AccountType? accountType = null);
        Task<Account> Update(Account account);
        Task Delete(int account);
        List<DailyBalanceList> GetDailyBalances(int? accountId = null, AccountType? accountType = null, DateTime? from = null, DateTime? to = null);
    }
}