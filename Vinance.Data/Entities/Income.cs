using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;
    using Categories;

    public class Income : Transaction
    {
        [Required]
        [ForeignKey("To")]
        public int ToId { get; set; }

        [Required]
        [ForeignKey("IncomeCategory")]
        public int IncomeCategoryId { get; set; }

        public virtual Account To { get; set; }
        public virtual IncomeCategory IncomeCategory { get; set; }
    }
}