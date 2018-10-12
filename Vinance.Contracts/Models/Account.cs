using System.Collections.Generic;

namespace Vinance.Contracts.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        public int UserId { get; set; }

        public ICollection<Income> Incomes { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public ICollection<Transfer> TransfersFrom { get; set; }

        public ICollection<Transfer> TransfersTo { get; set; }
    }
}