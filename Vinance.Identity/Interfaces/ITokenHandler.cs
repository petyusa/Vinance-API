
namespace Vinance.Identity.Interfaces
{
    using Contracts.Models.Identity;
    using Entities;

    public interface ITokenHandler
    {
        AuthToken GenerateToken(VinanceUser user, RefreshToken refreshToken = null);
    }
}