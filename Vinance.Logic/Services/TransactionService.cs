using System;
using System.Collections.Generic;
using Vinance.Contracts.Models;
using Vinance.Contracts.Models.Categories;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;

    public class TransactionService : ITransactionService
    {
        public IEnumerable<Payment> GetPayments()
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