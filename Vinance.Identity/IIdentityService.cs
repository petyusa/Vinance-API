using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Vinance.Identity
{
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;

    public interface IIdentityService
    {
        Task<TokenResult> GetToken(LoginModel loginModel);
        Task<IdentityResult> Register(VinanceUser user, string password);
        Task<bool> ChangePassword(PasswordChangeModel changeModel);
        Task<string> ResetPassword(string email);
    }
}