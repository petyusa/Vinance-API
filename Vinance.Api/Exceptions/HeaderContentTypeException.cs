using System;
using System.Runtime.Serialization;

namespace Vinance.Api.Exceptions
{
    public class HeaderContentTypeException : Exception
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