using AutoMapper;
using IdentityService.Api.Data.Models;
using IdentityService.Api.Data.Repositories;
using IdentityService.Api.Models;
using IdentityService.Api.Models.Users;
using Serilog;

namespace IdentityService.Api.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this._mapper = mapper;
        }
        public async Task AddUser(CreateUserRequestModel request)
        {
            var user = _mapper.Map<User>(request);
            user.Salt = GenerateSalt();
            await _userRepository.CreateUser(user);
        }

        public async Task<GetUserResponseModel> GetUser(GetUserRequestModel request)
        {
            var user = await _userRepository.GetUser(request?.Username);
            Log.Information(user.Username);
            return _mapper.Map<GetUserResponseModel>(user);
        }

        public string GenerateSalt()
        {
            Random rand = new Random();

            // Choosing the size of string
            // Using Next() string
            int stringlen = rand.Next(4, 10);
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {

                // Generating a random number.
                randValue = rand.Next(0, 26);

                // Generating random character by converting
                // the random number into character.
                letter = Convert.ToChar(randValue + 65);

                // Appending the letter to string.
                str = str + letter;
            }
            return str;
        }
    }
}