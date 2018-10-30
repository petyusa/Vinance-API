namespace Vinance.Contracts.Models
{
    using BaseModels;

    public class Expense : Transaction
    {
        public Account From { get; set; }
        public int FromId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}