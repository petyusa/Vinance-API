using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities.Base
{
    public class Category : BaseEntity
    {
        [Required]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(100)]
        public string Name { get; set; }
    }
}