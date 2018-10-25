namespace Vinance.Contracts.Models.Identity
{
    public class EmailChangeModel
    {
        public string NewEmail { get; set; }
        public string Token { get; set; }
    }
}