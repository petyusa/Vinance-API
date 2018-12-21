using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;

    [Table("Transfers", Schema = "Vinance")]
    public class Transfer : BaseEntity
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
        [ForeignKey("From")]
        public int FromId { get; set; }

        [Required]
        [ForeignKey("To")]
        public int ToId { get; set; }

        public virtual Account From { get; set; }
        public virtual Account To { get; set; }
    }
}