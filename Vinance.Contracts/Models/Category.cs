﻿namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Enums;

    public class Category : BaseModel
    {
        public string Name { get; set; }
        public int Balance { get; set; }
        public int MonthlyLimit { get; set; }
        public CategoryType Type { get; set; }
        public bool CanBeDeleted { get; set; }
    }
}