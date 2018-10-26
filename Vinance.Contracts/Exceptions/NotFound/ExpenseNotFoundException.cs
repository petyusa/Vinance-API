using System;

namespace Vinance.Contracts.Exceptions.NotFound
{
    using Base;

    public class ExpenseNotFoundException : VinanceNotFoundException
    {
        public ExpenseNotFoundException()
        {
        }

        public ExpenseNotFoundException(string message) : base(message)
        {
        }

        public ExpenseNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}