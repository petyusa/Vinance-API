using System;

namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Enums;

    public class Transfer : BaseModel
    {
        public DateTime Date { get; set; }
        public TransferType TransferType { get; set; }
        public int Amount { get; set; }
        public string Comment { get; set; }
        public Account From { get; set; }
        public int FromId { get; set; }
        public Account To { get; set; }
        public int ToId { get; set; }
    }
}