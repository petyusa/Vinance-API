using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("PaymentCategories", Schema = "Vinance")]
    public class PaymentCategory : Category
    {
    }
}