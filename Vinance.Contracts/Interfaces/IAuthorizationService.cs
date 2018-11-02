using System.Threading.Tasks;

namespace Vinance.Contracts.Interfaces
{
    using Models.BaseModels;

    public interface IAuthorizationService
    {
        Task HandleCreateUpdateAsync(BaseModel resource);
        Task HandleGetDeleteAsync<T>(int entityId) where T : BaseModel;
    }
}