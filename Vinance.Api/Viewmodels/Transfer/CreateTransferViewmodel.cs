using System;

namespace Vinance.Api.Viewmodels.Transfer
{
    using Contracts.Enums;

    /// <summary>
    /// The basic model for creating a transfer.
    /// </summary>
    public class CreateTransferViewmodel
    {
        /// <summary>
        /// The date of the transfer.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The type of the transfer.
        /// </summary>
        public TransferType TransferType { get; set; }

        /// <summary>
        /// The amount of the transfer.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The comment for the transfer.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The id of the account where the amount should be deducted from.
        /// </summary>
        public int FromId { get; set; }

        /// <summary>
        /// The id of the account where the amount should be added to.
        /// </summary>
        public int ToId { get; set; }
    }
}