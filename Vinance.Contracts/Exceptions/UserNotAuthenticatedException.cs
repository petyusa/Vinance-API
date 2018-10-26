using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class UserNotAuthenticatedException : VinanceException
    {
        public UserNotAuthenticatedException()
        {
        }

        public UserNotAuthenticatedException(string message) : base(message)
        {
        }

        public UserNotAuthenticatedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}