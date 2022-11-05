namespace IdentityService.Api.Services
{
    public interface IAuthManagerService
    {
        Task<string> Authenticate(string username, string password);
    }
}