using System;

namespace Vinance.Contracts.Exceptions
{
    public class ExpenseNotFoundException : VinanceException
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