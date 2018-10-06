namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Income : Transaction
    {
        public virtual Account To { get; set; }
        public virtual IncomeCategory IncomeCategory { get; set; }
    }
}