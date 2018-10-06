namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Payment : Transaction
    {
        public Account From { get; set; }
        public PaymentCategory PaymentCategory { get; set; }
    }
}