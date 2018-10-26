using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class TransferNotFoundAcception : VinanceException
    {
        public TransferNotFoundAcception()
        {
        }

        public TransferNotFoundAcception(string message) : base(message)
        {
        }

        public TransferNotFoundAcception(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}