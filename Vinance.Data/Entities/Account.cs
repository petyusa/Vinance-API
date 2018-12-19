using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vinance.Data.Entities
{
    using Base;

    [Table("Accounts", Schema = "Vinance")]
    public class Account : BaseEntity
    {
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsSaving { get; set; }

        public int OpeningBalance { get; set; }

        public int Balance()
        {
            if (Incomes != null &&
                Expenses != null &&
                TransfersTo != null &&
                TransfersFrom != null)
            {
                return OpeningBalance +
                    Incomes.Sum(i => i.Amount) -
                    Expenses.Sum(e => e.Amount) -
                    TransfersFrom.Sum(t => t.Amount) +
                    TransfersTo.Sum(t => t.Amount);
            }

            return 0;
        }

        public bool CanBeDeleted => (Incomes == null || !Incomes.Any()) &&
                                    (Expenses == null || !Expenses.Any()) &&
                                    (TransfersTo == null || !TransfersTo.Any()) &&
                                    (TransfersFrom == null || !TransfersFrom.Any());

        public virtual IEnumerable<Income> Incomes { get; set; }

        public virtual IEnumerable<Expense> Expenses { get; set; }

        [InverseProperty("From")]
        public virtual IEnumerable<Transfer> TransfersFrom { get; set; }

        [InverseProperty("To")]
        public virtual IEnumerable<Transfer> TransfersTo { get; set; }
    }
}