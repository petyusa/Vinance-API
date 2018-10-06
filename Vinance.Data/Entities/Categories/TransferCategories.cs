using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("TransferCategories", Schema = "Vinance")]
    public class TransferCategory : Category
    {
    }
}