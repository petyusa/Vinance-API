namespace Vinance.Contracts.Models
{
    using BaseModels;
    using Categories;

    public class Transfer : Transaction
    {
        public Account From { get; set; }
        public Account To { get; set; }
        public TransferCategory TransferCategory { get; set; }
    }
}