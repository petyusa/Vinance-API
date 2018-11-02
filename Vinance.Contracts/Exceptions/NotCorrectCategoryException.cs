using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class NotCorrectCategoryException : VinanceException
    {
        public NotCorrectCategoryException()
        {
        }

        public NotCorrectCategoryException(string message) : base(message)
        {
        }

        public NotCorrectCategoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}