namespace IdentityService.Api.Models
{
    public class AuthenticateRequestModel
    {
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
