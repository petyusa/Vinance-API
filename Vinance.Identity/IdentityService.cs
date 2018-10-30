using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Vinance.Contracts.Exceptions;

namespace Vinance.Identity
{
    using Contracts.Exceptions.NotFound;
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<VinanceUser> _userManager;
        private readonly ClaimsPrincipal _user;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<VinanceUser> userManager, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _user = contextAccessor.HttpContext.User;
        }

        public async Task<IdentityResult> Register(RegisterModel model, string password)
        {
            var user = _mapper.Map<VinanceUser>(model);
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<TokenResult> GetAccessToken(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            var result = new TokenResult { Succeeded = false };

            if (passwordCheckResult)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateToken(user, roles);
                result.Succeeded = true;
                result.Token = token;
                return result;
            }

            return result;
        }

        public async Task<IdentityResult> ChangePassword(PasswordChangeModel changeModel)
        {
            var user = await _userManager.GetUserAsync(_user);
            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, changeModel.OldPassword, changeModel.NewPassword);
            return passwordChangeResult;
        }

        public async Task<string> GetPasswordResetToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPassword(PasswordResetModel resetModel)
        {
            var user = await _userManager.FindByEmailAsync(resetModel.Email);
            var result = await _userManager.ResetPasswordAsync(user, resetModel.Token, resetModel.Password);
            return result;
        }

        public async Task<string> GetEmailChangeToken(string newEmail)
        {
            var user = await _userManager.GetUserAsync(_user);
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            return token;
        }

        public async Task<IdentityResult> ChangeEmail(EmailChangeModel emailChangeModel)
        {
            var user = await _userManager.GetUserAsync(_user);
            var result = await _userManager.ChangeEmailAsync(user, emailChangeModel.NewEmail, emailChangeModel.Token);
            return result;
        }

        public async Task<VinanceUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_user);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public Guid GetCurrentUserId()
        {
            var user = _userManager.GetUserAsync(_user).Result;
            if (user == null)
            {
                throw new UserNotAuthenticatedException("You are not authorized to make this request");
            }
            return user.Id;
        }

        private string GenerateToken(VinanceUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(30)).ToUnixTimeSeconds().ToString()),
            };

            claims.AddRange(roles.Select(role => new Claim("rol", role)));

            var header =
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            "the secret that needs to be at least 16characeters long for HmacSha256")),
                    SecurityAlgorithms.HmacSha256));
            var payLoad = new JwtPayload(claims);
            var token = new JwtSecurityToken(header, payLoad);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
