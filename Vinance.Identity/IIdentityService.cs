using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Vinance.Identity
{
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;

    public interface IIdentityService
    {
        Task<TokenResult> GetAccessToken(LoginModel loginModel);
        Task<IdentityResult> Register(VinanceUser user, string password);
        Task<IdentityResult> ChangePassword(PasswordChangeModel changeModel);
        Task<string> GetPasswordResetToken(string email);
        Task<IdentityResult> ResetPassword(PasswordResetModel resetModel);
        Task<string> GetEmailChangeToken(string newEmail);
        Task<IdentityResult> ChangeEmail(EmailChangeModel emailChangeModel);
        Task<VinanceUser> GetCurrentUser();
        Guid GetCurrentUserId();
    }
}