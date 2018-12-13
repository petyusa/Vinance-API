using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface IIncomeService
    {
        Task<IEnumerable<Income>> GetAll(int? categoryId = null, DateTime? from = null, DateTime? to = null, string order = "date_desc");
        Task<Income> Create(Income account);
        Task<IEnumerable<Income>> Upload(StreamReader stream);
        Task<Income> GetById(int accountId);
        Task<Income> Update(Income account);
        Task Delete(int incomeId);
    }
}