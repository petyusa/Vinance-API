namespace Vinance.Contracts.Models
{
    using BaseModels;

    public class Transfer : Transaction
    {
        public Account From { get; set; }
        public int FromId { get; set; }
        public Account To { get; set; }
        public int ToId { get; set; }
    }
}