using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class NotAccountOwnerException : VinanceException
    {
        public NotAccountOwnerException()
        {
        }

        public NotAccountOwnerException(string message) : base(message)
        {
        }

        public NotAccountOwnerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}