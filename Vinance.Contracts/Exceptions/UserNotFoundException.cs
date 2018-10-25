using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class UserNotFoundException : VinanceException
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}