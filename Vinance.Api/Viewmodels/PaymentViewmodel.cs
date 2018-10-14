using System;

namespace Vinance.Api.Viewmodels
{
    public class PaymentViewmodel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public int FromId { get; set; }

        public int PaymentCategoryId { get; set; }
    }
}
