using System;
using System.Collections.Generic;

namespace Vinance.Contracts.Models.Helpers
{
    public class DailyBalanceList
    {
        public string AccountName { get; set; }
        public Dictionary<DateTime, int> DailyBalances { get; set; }
    }
}