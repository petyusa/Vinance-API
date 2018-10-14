using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface IIncomeService
    {
        Task<IEnumerable<Income>> GetAll();
        Task<Income> Create(Income account);
        Task<Income> Get(int accountId);
        Task<Income> Update(Income account);
        Task<bool> Delete(int incomeId);
    }
}