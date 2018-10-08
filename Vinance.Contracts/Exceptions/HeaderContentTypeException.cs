using System;

namespace Vinance.Contracts.Exceptions
{
    public class HeaderContentTypeException : VinanceException
    {
        public HeaderContentTypeException()
        {
        }

        public HeaderContentTypeException(string message) : base(message)
        {
        }

        public HeaderContentTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}