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
        private readonly IDictionary<string, string> adminUsers = new Dictionary<string, string>() {
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

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetUser(username);
            // TODO : for timebeing
            if (user is null )
            {
                if (adminUsers.Any(a => a.Key == username && a.Value == password))
                {
                    user = new Data.Models.User();
                    user.Username = username;
                }
                else
                {
                    return null;
                }
            }

            var tokenKey = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var expiry = DateTime.Now.AddSeconds(_authSettings.ValidDuration);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var additionalClaims = new Dictionary<string, object>();
            additionalClaims.Add("mobile", user?.Mobile);
            additionalClaims.Add("role", "user");
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("username", user.Username)
                }),
                Claims = additionalClaims,
                Issuer = _authSettings.Issuer,
                Expires = expiry,
                Audience = _authSettings.Audience,
                SigningCredentials = credentials,
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            await _loginStateRepository.SaveUserLoginStateAsync(new Data.Models.LoginStateModel()
            {
                Token = token,
                Username = username
            });

            return token;
        }


    }
}
