using System;

namespace Vinance.Contracts.Exceptions.NotFound
{
    using Base;

    public class TransferNotFoundException : VinanceNotFoundException
    {
        public TransferNotFoundException()
        {
        }

        public TransferNotFoundException(string message) : base(message)
        {
        }

        public TransferNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}