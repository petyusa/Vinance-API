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
        Task<IdentityResult> Register(RegisterModel user, string password);
        Task<IdentityResult> ChangePassword(PasswordChangeModel changeModel);
        Task<TokenResult> GetPasswordResetToken(string email);
        Task<IdentityResult> ResetPassword(PasswordResetModel resetModel);
        Task<TokenResult> GetEmailChangeToken(string newEmail);
        Task<IdentityResult> ChangeEmail(EmailChangeModel emailChangeModel);
        Task<VinanceUser> GetUserByName(string userName);
        Task<VinanceUser> GetCurrentUser();
        Guid GetCurrentUserId();
    }
}