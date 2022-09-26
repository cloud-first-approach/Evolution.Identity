using IdentityService.Api.Data.Models;

namespace IdentityService.Api.Data.Repositories
{
    public interface ILoginStateRepository
    {
        public Task SaveUserLoginStateAsync(LoginStateModel userState);

        public Task<LoginStateModel> GetUserLoginStateAsync(string username);
    }
}
