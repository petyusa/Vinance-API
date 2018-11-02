using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities.Base
{
    public class Transaction : BaseEntity
    {
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        [Column(TypeName = "NVARCHAR(256)")]
        public string Comment { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}