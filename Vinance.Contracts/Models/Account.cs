using System.Collections.Generic;
using Vinance.Contracts.Enums;

namespace Vinance.Contracts.Models
{
    public class Account : BaseModels.BaseModel
    {
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public int OpeningBalance { get; set; }
        public int Balance { get; set; }
        public bool IsActive { get; set; }
        public bool CanBeDeleted { get; set; }
        public ICollection<Income> Incomes { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Transfer> TransfersTo { get; set; }
    }
}