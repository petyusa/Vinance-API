namespace Vinance.Contracts.Models
{
    using BaseModels;

    public class Expense : Transaction
    {
        public Account From { get; set; }
        public int FromId { get; set; }
    }
}