using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("ExpenseCategories", Schema = "Vinance")]
    public class ExpenseCategory : Category
    {
        public int Balance()
        {
            return Expenses?.Sum(e => e.Amount) ?? 0;
        }

        public virtual IEnumerable<Expense> Expenses { get; set; }
    }
}