using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class IncomeNotFoundException : VinanceException
    {
        public IncomeNotFoundException()
        {
        }

        public IncomeNotFoundException(string message) : base(message)
        {
        }

        public IncomeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}