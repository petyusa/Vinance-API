using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPayments();
    }
}