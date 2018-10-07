using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface IPaymentService
    {
        Task<Payment> Create(Payment payment);
        Task<IEnumerable<Payment>> GetAll();
        Task<Payment> GetById(int paymentId);
        Task<Payment> Update(Payment payment);
        Task<bool> Delete(int paymentId);
    }
}