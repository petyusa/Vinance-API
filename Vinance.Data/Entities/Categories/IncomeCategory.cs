using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("IncomeCategories", Schema = "Vinance")]
    public class IncomeCategory : Category
    {
    }
}