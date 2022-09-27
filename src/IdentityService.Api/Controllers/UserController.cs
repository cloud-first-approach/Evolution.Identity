using IdentityService.Api.Data.Models;
using IdentityService.Api.Data.Repositories;
using IdentityService.Api.Middlewares;
using IdentityService.Api.Models.Users;
using IdentityService.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        public async Task<IActionResult> Get()
        {
            var principal = HttpContext.Items["Principal"];
            
            if (principal == null)
            {
                return Unauthorized();
            }
            var claims = (BasePrincipal)principal;
            return Ok(await _userManager.GetUser(new GetUserRequestModel() { Username = claims?.Subject }));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRequestModel request)
        {
            await _userManager.AddUser(request);
            return Created("test", request);
        }
    }
}