using System;

namespace Vinance.Contracts.Exceptions.NotFound
{
    using Base;

    public class UserNotFoundException : VinanceNotFoundException
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