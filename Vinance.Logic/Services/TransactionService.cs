using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;
    using Contracts.Models;
    using Contracts.Models.Categories;

    public class TransactionService : ITransactionService
    {
        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return new List<Payment>
            {
                new Payment
                {
                    Amount = 456,
                    Comment = "nocomment",
                    Date = DateTime.Now,
                    From = new Account { Balance = 1000, Id = 1, Name = "MyAccount"},
                    Id = 1,
                    PaymentCategory = new PaymentCategory { Id = 1, Name = "MyPaymentCategory"}
                },
                new Payment
                {
                    Amount = 123,
                    Comment = "nocomment",
                    Date = DateTime.Now,
                    From = new Account { Balance = 1000, Id = 1, Name = "MyAccount"},
                    Id = 1,
                    PaymentCategory = new PaymentCategory { Id = 1, Name = "MyPaymentCategory"}
                }
            };
        }
    }
}