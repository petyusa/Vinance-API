﻿using System.Collections.Generic;
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

        public int UserId { get; set; }

        public int OpeningBalance { get; set; }

        public int Balance()
        {
            return OpeningBalance +
                Incomes.Sum(i => i.Amount) -
                Expenses.Sum(e => e.Amount) -
                TransfersFrom.Sum(t => t.Amount) +
                TransfersTo.Sum(t => t.Amount);
        }

        public virtual IEnumerable<Income> Incomes { get; set; }

        public virtual IEnumerable<Expense> Expenses { get; set; }

        [InverseProperty("From")]
        public virtual IEnumerable<Transfer> TransfersFrom { get; set; }

        [InverseProperty("To")]
        public virtual IEnumerable<Transfer> TransfersTo { get; set; }
    }
}