namespace IdentityService.Api.Services
{
    public interface IAuthManagerService
    {
        string Authenticate(string username, string password);
    }
}