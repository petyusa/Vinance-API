using System;

namespace Vinance.Contracts.Exceptions
{
    using Base;

    public class TransferNotFoundException : VinanceException
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