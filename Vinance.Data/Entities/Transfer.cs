using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;

    [Table("Transfers", Schema = "Vinance")]
    public class Transfer : Transaction
    {
        [Required]
        [ForeignKey("From")]
        public int FromId { get; set; }

        [Required]
        [ForeignKey("To")]
        public int ToId { get; set; }

        public virtual Account From { get; set; }
        public virtual Account To { get; set; }
    }
}