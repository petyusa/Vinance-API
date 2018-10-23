﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Vinance.Identity
{
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<VinanceUser> _userManager;
        private readonly ClaimsPrincipal _user;

        public IdentityService(UserManager<VinanceUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _user = contextAccessor.HttpContext.User;
        }

        public async Task<IdentityResult> Register(VinanceUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<TokenResult> GetToken(LoginModel loginModel)
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

        public async Task<bool> ChangePassword(PasswordChangeModel changeModel)
        {
            var user = await _userManager.GetUserAsync(_user);
            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, changeModel.OldPassword, changeModel.NewPassword);
            return passwordChangeResult.Succeeded;
        }

        public async Task<string> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }


        private string GenerateToken(VinanceUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
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