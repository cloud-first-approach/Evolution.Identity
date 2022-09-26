using IdentityService.Api.AppSettings;
using IdentityService.Api.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Api.Services
{
    public class AuthManagerService : IAuthManagerService
    {
        private readonly IDictionary<string, string> authenticatedUsers = new Dictionary<string, string>() {
            { "sourabh", "sourabh" },
            { "sharad","sharad" }
        };

        private readonly AuthSettings _authSettings;
        private readonly ILoginStateRepository _loginStateRepository;

        public AuthManagerService(IOptions<AuthSettings> authSettingOptions, ILoginStateRepository loginStateRepository)
        {
            _authSettings = authSettingOptions.Value;
            _loginStateRepository = loginStateRepository;
        }

        public string Authenticate(string username, string password)
        {
            if (!authenticatedUsers.Any(a => a.Key == username && a.Value == password))
            {
                return null;
            }

            var tokenKey = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var expiry = DateTime.Now.AddSeconds(_authSettings.ValidDuration);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, username)
                }),
                Issuer = _authSettings.Issuer,
                Expires = expiry,
                Audience = _authSettings.Audience,
                SigningCredentials = credentials,
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            _loginStateRepository.SaveUserLoginStateAsync(new Data.Models.LoginStateModel()
            {
                Token = token,
                Username = username
            });

            return token;
        }


    }
}
