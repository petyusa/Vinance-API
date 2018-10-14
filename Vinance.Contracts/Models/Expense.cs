namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Expense : Transaction
    {
        public Account From { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
    }
}