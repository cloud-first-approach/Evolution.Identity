using IdentityService.Api.Models;
using IdentityService.Api.Models.Users;

namespace IdentityService.Api.Services
{
    public interface IUserManager
    {
        Task<GetUserResponseModel> GetUser(GetUserRequestModel token);
        Task AddUser(CreateUserRequestModel token);
    }
}