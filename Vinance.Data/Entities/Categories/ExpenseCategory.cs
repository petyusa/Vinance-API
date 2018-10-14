using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("ExpenseCategories", Schema = "Vinance")]
    public class ExpenseCategory : Category
    {
    }
}