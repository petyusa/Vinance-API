using System;
using System.Collections.Generic;

namespace Vinance.Contracts.Models.Helpers
{
    public class CategoryStatistics
    {
        public DateTime Date { get; set; }
        public IEnumerable<CategoryStatisticsItem> Items { get; set; }
    }

    public class CategoryStatisticsItem
    {
        public int Balance { get; set; }
        public string Name { get; set; }
    }
}