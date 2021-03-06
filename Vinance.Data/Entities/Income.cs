﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinance.Data.Entities
{
    using Base;

    [Table("Incomes", Schema = "Vinance")]
    public class Income : Transaction
    {
        [Required]
        [ForeignKey("To")]
        public int ToId { get; set; }

        public virtual Account To { get; set; }
    }
}