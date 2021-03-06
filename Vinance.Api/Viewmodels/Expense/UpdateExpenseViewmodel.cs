﻿using System;

namespace Vinance.Api.Viewmodels.Expense
{
    public class UpdateExpenseViewmodel
    {
        /// <summary>
        /// The id of the expense.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date of the expense.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount of the expense.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Comment for the expense.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The id of the account from where the amount should be deducted.
        /// </summary>
        public int FromId { get; set; }

        /// <summary>
        /// The expense-category for the expense.
        /// </summary>
        public int CategoryId { get; set; }
    }
}