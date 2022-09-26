using IdentityService.Api.Data.Models;

namespace IdentityService.Api.Data.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(User username);
        Task<User> GetUser(string username);
    }
}
