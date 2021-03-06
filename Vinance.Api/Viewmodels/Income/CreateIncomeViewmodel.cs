﻿using System;

namespace Vinance.Api.Viewmodels.Income
{
    /// <summary>
    /// The basic model for creating an income.
    /// </summary>
    public class CreateIncomeViewmodel
    {
        /// <summary>
        /// The date of the income.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The amount of the income.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The comment for the income.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The id of the account where the amount should be added.
        /// </summary>
        public int ToId { get; set; }

        /// <summary>
        /// The income-category of the account.
        /// </summary>
        public int CategoryId { get; set; }
    }
}