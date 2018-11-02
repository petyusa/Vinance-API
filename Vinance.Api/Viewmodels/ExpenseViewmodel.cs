using System;

namespace Vinance.Api.Viewmodels
{
    public class ExpenseViewmodel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public int FromId { get; set; }

        public string FromName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
