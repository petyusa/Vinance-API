using System;

namespace Vinance.Contracts.Exceptions.NotFound
{
    using Base;

    public class IncomeNotFoundException : VinanceNotFoundException
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