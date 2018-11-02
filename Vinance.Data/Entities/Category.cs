using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vinance.Data.Entities.Base;
using Vinance.Data.Enums;

namespace Vinance.Data.Entities
{
    [Table("Categories", Schema = "Vinance")]
    public class Category : BaseEntity
    {
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        public CategoryType Type { get; set; }
    }
}