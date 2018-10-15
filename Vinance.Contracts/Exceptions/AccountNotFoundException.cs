using System;

namespace Vinance.Contracts.Exceptions
{
    public class AccountNotFoundException : VinanceException
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