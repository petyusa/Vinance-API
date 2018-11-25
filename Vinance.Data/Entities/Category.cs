using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;
    using Enums;

    [Table("Categories", Schema = "Vinance")]
    public class Category : BaseEntity
    {
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        public CategoryType Type { get; set; }
    }
}