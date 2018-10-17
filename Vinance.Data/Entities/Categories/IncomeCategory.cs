using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("IncomeCategories", Schema = "Vinance")]
    public class IncomeCategory : Category
    {
        public int Balance()
        {
            return Incomes?.Sum(e => e.Amount) ?? 0;
        }

        public IEnumerable<Income> Incomes { get; set; }
    }
}