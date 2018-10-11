namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Income : Transaction
    {
        public int ToId { get; set; }
        public Account To { get; set; }
        public int IncomeCategoryId { get; set; }
        public IncomeCategory IncomeCategory { get; set; }
    }
}