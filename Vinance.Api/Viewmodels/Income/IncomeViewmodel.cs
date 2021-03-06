﻿using System;

namespace Vinance.Api.Viewmodels.Income
{
    public class IncomeViewmodel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public int ToId { get; set; }

        public string ToName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}