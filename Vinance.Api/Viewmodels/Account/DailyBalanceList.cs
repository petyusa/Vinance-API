using System;
using System.Collections.Generic;

namespace Vinance.Api.Viewmodels.Account
{
    /// <summary>
    /// Daily balance list for an account.
    /// </summary>
    public class DailyBalanceList
    {
        /// <summary>
        /// The name of the account.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Dictionary of daily balances, the key is the date, the value is the balance.
        /// </summary>
        public Dictionary<DateTime, int> DailyBalances { get; set; }
    }
}