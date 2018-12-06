using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Vinance.Identity.Services
{
    using Contracts.Interfaces;
    using Contracts.Models.Identity;
    using Entities;
    using Helpers;
    using Interfaces;

    public class TokenHandler : ITokenHandler
    {
        private readonly IFactory<IdentityContext> _factory;
        private readonly IConfiguration _configuration;

        public TokenHandler(IFactory<IdentityContext> factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        public AuthToken GenerateToken(VinanceUser user, RefreshToken refreshToken = null)
        {
            var options = GetOptions();
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

            using (var context = _factory.CreateDbContext())
            {
                if (refreshToken == null)
                {
                    refreshToken = new RefreshToken()
                    {
                        UserId = user.Id,
                        Token = Guid.NewGuid().ToString("N"),
                    };

                    var tokenModel = context.RefreshTokens.SingleOrDefault(i => i.UserId == refreshToken.UserId);
                    if (tokenModel != null)
                    {
                        context.RefreshTokens.Remove(tokenModel);
                        context.SaveChanges();
                    }
                    context.RefreshTokens.Add(refreshToken);
                    context.SaveChanges();
                }

                refreshToken.IssuedUtc = now;
                refreshToken.ExpiresUtc = now.Add(options.RefreshTokenExpiration);
                context.SaveChanges();
            }

            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims.ToArray(),
                notBefore: now,
                expires: now.Add(options.Expiration),
                signingCredentials: options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var result = new AuthToken
            {
                AccessToken = encodedJwt,
                RefreshToken = refreshToken.Token,
                Expires = now.AddSeconds(options.Expiration.TotalSeconds),
                RefreshTokenExpiresIn = (int)options.RefreshTokenExpiration.TotalSeconds,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };
            return result;
        }

        private TokenProviderOptions GetOptions()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("TokenAuthentication:SecretKey").Value));

            return new TokenProviderOptions
            {
                Audience = _configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = _configuration.GetSection("TokenAuthentication:Issuer").Value,
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(_configuration.GetSection("TokenAuthentication:ExpirationMinutes").Value)),
                RefreshTokenExpiration = TimeSpan.FromDays(Convert.ToInt32(_configuration.GetSection("TokenAuthentication:RefreshTokenExpirationDays").Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };
        }
    }
}