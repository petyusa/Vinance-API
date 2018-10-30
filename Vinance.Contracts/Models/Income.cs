namespace Vinance.Contracts.Models
{
    using BaseModels;

    public class Income : Transaction
    {
        public int ToId { get; set; }
        public Account To { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}