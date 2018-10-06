using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;

    public class Account : BaseEntity
    {

        [Required]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Range(1, int.MaxValue)]
        public int Balance { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}