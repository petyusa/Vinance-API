namespace Vinance.Contracts.Models.Identity
{
    public class PasswordChangeModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}