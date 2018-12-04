using System;
using Microsoft.IdentityModel.Tokens;

namespace Vinance.Identity.Helpers
{
    public class TokenProviderOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}