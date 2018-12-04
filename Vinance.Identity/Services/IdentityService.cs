using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Vinance.Identity.Services
{
    using Contracts.Exceptions;
    using Contracts.Exceptions.Base;
    using Contracts.Exceptions.NotFound;
    using Contracts.Interfaces;
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;
    using Entities;
    using Interfaces;

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<VinanceUser> _userManager;
        private readonly IFactory<IdentityContext> _factory;
        private readonly ITokenHandler _tokenHandler;
        private readonly ClaimsPrincipal _user;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<VinanceUser> userManager, IFactory<IdentityContext> factory, IHttpContextAccessor contextAccessor, ITokenHandler tokenHandler, IMapper mapper)
        {
            _userManager = userManager;
            _factory = factory;
            _user = contextAccessor.HttpContext.User;
            _tokenHandler = tokenHandler;
            _mapper = mapper;
        }

        public async Task<IdentityResult> Register(RegisterModel model, string password)
        {
            var user = _mapper.Map<VinanceUser>(model);

            return await _userManager.CreateAsync(user, password);
        }

        public async Task<AuthToken> GetToken(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (!user.EmailConfirmed)
            {
                throw new VinanceException("Email address not confirmed");
            }
            var passwordCheckResult = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            if (!passwordCheckResult)
            {
                throw new UserException("Invalid username or password");
            }

            var token = _tokenHandler.GenerateToken(user);

            return token;
        }

        public AuthToken RefreshToken(string refreshToken)
        {
            RefreshToken refreshTokenModel;
            using (var context = _factory.CreateDbContext())
            {
                refreshTokenModel = context.RefreshTokens
                    .Include(x => x.User)
                    .SingleOrDefault(i => i.Token == refreshToken);
            }

            if (refreshTokenModel == null)
            {
                throw new UserNotAuthenticatedException();
            }

            var user = refreshTokenModel.User;
            var token = _tokenHandler.GenerateToken(user, refreshTokenModel);
            return token;
        }

        public async Task<IdentityResult> ChangePassword(PasswordChangeModel changeModel)
        {
            var user = await _userManager.GetUserAsync(_user);
            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, changeModel.OldPassword, changeModel.NewPassword);
            return passwordChangeResult;
        }

        public async Task<TokenResult> GetPasswordResetToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException($"No user found with email: {email}");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = new TokenResult
            {
                Succeeded = true,
                Token = token
            };
            return result;
        }

        public async Task<IdentityResult> ResetPassword(PasswordResetModel resetModel)
        {
            var user = await _userManager.FindByEmailAsync(resetModel.Email);
            var result = await _userManager.ResetPasswordAsync(user, resetModel.Token, resetModel.Password);
            return result;
        }

        public async Task<TokenResult> GetEmailChangeToken(string newEmail)
        {
            var user = await _userManager.GetUserAsync(_user);
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = new TokenResult
            {
                Succeeded = true,
                Token = token
            };
            return result;
        }

        public async Task<TokenResult> GetEmailConfirmationToken(string email)
        {
            var result = new TokenResult { Succeeded = false };
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedBytes = Encoding.Unicode.GetBytes(token);
            var encodedToken = Convert.ToBase64String(encodedBytes);
            result.Succeeded = true;
            result.Token = encodedToken;
            return result;
        }

        public async Task<bool> ConfirmEmail(EmailConfirmationModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var decodedBytes = Convert.FromBase64String(model.Token);
            var token = Encoding.Unicode.GetString(decodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
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

        public async Task<VinanceUser> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UserNotAuthenticatedException("You are not authorized to make this request");
            }
            return user;
        }

        //private string GenerateToken(VinanceUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.FirstName),
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(30)).ToUnixTimeSeconds().ToString()),
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key more than 16 characters"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var header = new JwtHeader(creds);
        //    var payLoad = new JwtPayload(claims);
        //    var token = new JwtSecurityToken(header, payLoad);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
