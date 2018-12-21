using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface ITransferService
    {
        Task<Transfer> Create(Transfer transfer);
        Task<IEnumerable<Transfer>> Upload(StreamReader stream);
        Task<Transfer> GetById(int transferId);
        Task<IEnumerable<Transfer>> GetAll(int? accountId = null, DateTime? from = null, DateTime? to = null, string order = "date_desc");
        Task<Transfer> Update(Transfer transfer);
        Task Delete(int transfer);
    }
}