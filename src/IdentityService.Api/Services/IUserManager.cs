namespace IdentityService.Api.Services
{
    public interface IUserManager
    {
        string GetUser(string token);
        string AddUser(UserRequestModel token);
    }
}