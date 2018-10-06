using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;
    using Categories;

    [Table("Payments")]
    public class Payment : Transaction
    {
        [Required]
        [ForeignKey("From")]
        public int FromId { get; set; }

        [Required]
        [ForeignKey("PaymentCategory")]
        public int PaymentCategoryId { get; set; }

        public virtual Account From { get; set; }
        public virtual PaymentCategory PaymentCategory { get; set; }
    }
}