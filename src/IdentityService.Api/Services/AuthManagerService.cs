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
        private readonly IUserRepository _userRepository;

        public AuthManagerService(IOptions<AuthSettings> authSettingOptions, ILoginStateRepository loginStateRepository,IUserRepository userRepository)
        {
            _authSettings = authSettingOptions.Value;
            _loginStateRepository = loginStateRepository;
            this._userRepository = userRepository;
        }

        public string Authenticate(string username, string password)
        {
            var user = _userRepository.GetUser(username);
            // TODO : for timebeing
            if (user is null && !authenticatedUsers.Any(a => a.Key == username && a.Value == password))
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
                    new Claim("username", username)
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
