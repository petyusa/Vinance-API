using System.Collections.Generic;
using System.Threading.Tasks;
using Vinance.Contracts.Models.BaseModels;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface IAccountService
    {
        Task<Account> Create(Account account);
        Task<Account> Get(int accountId);
        Task<IEnumerable<Account>> GetAll();
        Task<Account> Update(Account account);
        Task<bool> Delete(int account);
    }
}