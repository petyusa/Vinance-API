using System.Collections.Generic;

namespace Vinance.Contracts.Interfaces
{
    using Models;

    public interface ITransactionService
    {
        IEnumerable<Payment> GetPayments();
    }
}