using System;

namespace Vinance.Contracts.Exceptions.Base
{
    public class VinanceException : Exception
    {
        public VinanceException()
        {
        }

        public VinanceException(string message) : base(message)
        {
        }

        public VinanceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}