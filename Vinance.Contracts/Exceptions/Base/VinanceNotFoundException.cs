using System;

namespace Vinance.Contracts.Exceptions.Base
{
    public class VinanceNotFoundException : VinanceException
    {
        public VinanceNotFoundException()
        {
        }

        public VinanceNotFoundException(string message) : base(message)
        {
        }

        public VinanceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}