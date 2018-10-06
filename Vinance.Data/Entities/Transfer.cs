using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;
    using Categories;

    public class Transfer : Transaction
    {
        [Required]
        [ForeignKey("From")]
        public int FromId { get; set; }

        [Required]
        [ForeignKey("To")]
        public int ToId { get; set; }

        [Required]
        [ForeignKey("TransferCategory")]
        public int TransferCategoryId { get; set; }

        public virtual Account From { get; set; }
        public virtual Account To { get; set; }
        public virtual TransferCategory TransferCategory { get; set; }
    }
}