using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface ITransferService
    {
        Task<Transfer> Create(Transfer transfer);
        Task<Transfer> GetById(int transferId);
        Task<IEnumerable<Transfer>> GetAll();
        Task<Transfer> Update(Transfer transfer);
        Task Delete(int transfer);
    }
}