using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class UserException : VinanceException
    {
        public UserException()
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}