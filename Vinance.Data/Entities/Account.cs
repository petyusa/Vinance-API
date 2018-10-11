using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using EntityFrameworkCore.Triggers;

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

        static Account()
        {
            Triggers<Income>.Inserting += entry =>
            {
                var account = entry.Context.Set<Account>().SingleOrDefault(a => a.Id == entry.Entity.ToId);
                if (account != null)
                    account.Balance += entry.Entity.Amount;
            };

            Triggers<Income>.Deleting += entry =>
            {
                var account = entry.Context.Set<Account>().SingleOrDefault(a => a.Id == entry.Entity.ToId);
                if (account != null)
                    account.Balance -= entry.Original.Amount;
            };
        }
    }
}