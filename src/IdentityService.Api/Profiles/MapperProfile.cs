using AutoMapper;
using IdentityService.Api.Data.Models;
using IdentityService.Api.Models.Users;

namespace IdentityService.Api.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, GetUserResponseModel>();
            CreateMap<CreateUserRequestModel, User>();
        }
    }
}
