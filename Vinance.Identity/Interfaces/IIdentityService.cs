﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Vinance.Identity.Interfaces
{
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;
    using Entities;

    public interface IIdentityService
    {
        Task<AuthToken> GetToken(LoginModel loginModel);
        AuthToken RefreshToken(string refreshToken);
        Task<IdentityResult> Register(RegisterModel user, string password);
        Task<IdentityResult> ChangePassword(PasswordChangeModel changeModel);
        Task<TokenResult> GetPasswordResetToken(string email);
        Task<IdentityResult> ResetPassword(PasswordResetModel resetModel);
        Task<TokenResult> GetEmailConfirmationToken(string email);
        Task<bool> ConfirmEmail(EmailConfirmationModel model);
        Task<TokenResult> GetEmailChangeToken(string newEmail);
        Task<IdentityResult> ChangeEmail(EmailChangeModel emailChangeModel);
        Task<VinanceUser> GetUserByName(string userName);
        Task<VinanceUser> GetCurrentUser();
        Guid GetCurrentUserId();
    }
}