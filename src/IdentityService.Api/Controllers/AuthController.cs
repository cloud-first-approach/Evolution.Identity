using IdentityService.Api.Models;
using IdentityService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthManagerService _jwtAuthManagerService;
        public AuthController(IAuthManagerService jwtAuthManagerService)
        {
            _jwtAuthManagerService = jwtAuthManagerService;
        }
        [HttpPost("connect/token")]
        public IActionResult Authenticate(AuthenticateRequestModel requestModel)
        {
           var authResponse =  _jwtAuthManagerService.Authenticate(requestModel.Username, requestModel.Password);
            if (authResponse is null)
                return Unauthorized();
            else
                return Ok(authResponse);

        }
    }
}
