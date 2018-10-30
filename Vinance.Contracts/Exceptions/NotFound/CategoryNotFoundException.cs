using System;

namespace Vinance.Contracts.Exceptions.NotFound
{
    using Base;

    public class CategoryNotFoundException : VinanceNotFoundException
    {
        public CategoryNotFoundException()
        {
        }

        public CategoryNotFoundException(string message) : base(message)
        {
        }

        public CategoryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}