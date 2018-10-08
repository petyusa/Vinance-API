using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface ITransferService
    {
        Task<Transfer> Create(Transfer transfer);
        Task<Transfer> Get(int transferId);
        Task<IEnumerable<Transfer>> GetAll();
        Task<Transfer> Update(Transfer transfer);
        Task<bool> Delete(int transfer);
    }
}