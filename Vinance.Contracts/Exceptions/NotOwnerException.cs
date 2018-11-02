using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class NotOwnerException : VinanceException
    {
        public NotOwnerException()
        {
        }

        public NotOwnerException(string message) : base(message)
        {
        }

        public NotOwnerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}