using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;
    using Categories;

    [Table("Expenses", Schema = "Vinance")]
    public class Expense : Transaction
    {
        [Required]
        [ForeignKey("From")]
        public int FromId { get; set; }

        [Required]
        [ForeignKey("ExpenseCategory")]
        public int ExpenseCategoryId { get; set; }

        public virtual Account From { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; }
    }
}