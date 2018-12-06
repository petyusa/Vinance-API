using System;

namespace Vinance.Contracts.Models.Identity
{
    public class AuthToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public int RefreshTokenExpiresIn { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}