namespace Vinance.Contracts.Models.Identity
{
    public class PasswordResetModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}