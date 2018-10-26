using System;

namespace Vinance.Contracts.Exceptions.NotFound
{
    using Base;

    public class AccountNotFoundException : VinanceNotFoundException
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message) : base(message)
        {
        }

        public AccountNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}