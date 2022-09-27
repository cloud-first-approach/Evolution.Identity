using AutoMapper;
using IdentityService.Api.Data.Models;
using IdentityService.Api.Data.Repositories;
using IdentityService.Api.Models;
using IdentityService.Api.Models.Users;

namespace IdentityService.Api.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            this._mapper = mapper;
        }
        public async Task AddUser(CreateUserRequestModel request)
        {
            await _userRepository.CreateUser(_mapper.Map<User>(request));
        }

        public async Task<GetUserResponseModel> GetUser(GetUserRequestModel request)
        {
            return _mapper.Map<GetUserResponseModel>(await _userRepository.GetUser(request?.Authorization));
        }
    }
}