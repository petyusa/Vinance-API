namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Income : Transaction
    {
        public Account To { get; set; }
        public IncomeCategory IncomeCategory { get; set; }
    }
}