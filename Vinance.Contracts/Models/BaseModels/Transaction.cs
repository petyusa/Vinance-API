using System;

namespace Vinance.Contracts.Models.BaseModels
{
    public class Transaction : BaseModel
    {
        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }
    }
}