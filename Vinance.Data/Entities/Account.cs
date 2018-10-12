using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int Balance { get; private set; }

        public virtual IEnumerable<Income> Incomes { get; set; }

        public virtual IEnumerable<Payment> Payments { get; set; }

        [InverseProperty("From")]
        public virtual IEnumerable<Transfer> TransfersFrom { get; set; }

        [InverseProperty("To")]
        public virtual IEnumerable<Transfer> TransfersTo { get; set; }
    }
}