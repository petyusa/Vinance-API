namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Transfer : Transaction
    {
        public Account From { get; set; }
        public int FromId { get; set; }
        public Account To { get; set; }
        public int ToId { get; set; }
        public TransferCategory TransferCategory { get; set; }
        public int TransferCategoryId { get; set; }
    }
}