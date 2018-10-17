using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vinance.Data.Entities.Categories
{
    using Base;

    [Table("TransferCategories", Schema = "Vinance")]
    public class TransferCategory : Category
    {
        public int Balance()
        {
            return Transfers?.Sum(e => e.Amount) ?? 0;
        }

        public IEnumerable<Transfer> Transfers { get; set; }
    }
}