using IdentityService.Api.Data.Models;
using IdentityService.Api.Data.Repositories;
using IdentityService.Api.Models.Users;
using IdentityService.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            this._userManager = userManager;
        }
        
        [HttpGet(Name = "GetUser")]
        public async Task<IActionResult> Get([FromQuery]GetUserRequestModel request)
        {
           return Ok(_userManager.GetUser(request));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRequestModel request)
        {
            return Ok(_userManager.AddUser(request));
        }
    }
}