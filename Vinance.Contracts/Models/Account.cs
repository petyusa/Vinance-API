namespace Vinance.Contracts.Models
{
    using BaseModels;

    public class Account : BaseModel
    {
        public string Name { get; set; }

        public int Balance { get; set; }

        public int UserId { get; set; }
    }
}