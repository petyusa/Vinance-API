using System;

namespace Vinance.Contracts.Models.BaseModels
{
    public class Transaction : BaseModel
    {
        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}