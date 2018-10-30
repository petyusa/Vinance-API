using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;

    [Table("Expenses", Schema = "Vinance")]
    public class Expense : Transaction
    {
        [Required]
        [ForeignKey("From")]
        public int FromId { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Account From { get; set; }
        public virtual Category Category { get; set; }
    }
}