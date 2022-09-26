using IdentityService.Api.Data.Models;
using IdentityService.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoginStateRepository identityUserRepository;

        public UserController(IHttpClientFactory httpClientFactory, ILoginStateRepository identityUserRepository)
        {
            this._httpClientFactory = httpClientFactory;
            this.identityUserRepository = identityUserRepository;
        }
        
        [HttpGet(Name = "GetState")]
        public async Task<IActionResult> Get([FromQuery]string username)
        {
            //var dapr = _httpClientFactory.CreateClient("dapr");
            //var msg = new HttpRequestMessage(HttpMethod.Get, "/v1.0/invoke/");
            return Ok(await this.identityUserRepository.GetUserLoginStateAsync(username));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string username)
        {
            await this.identityUserRepository.SaveUserLoginStateAsync(new LoginStateModel()
            {
                Username = username,
                Token = "",
                LastGenerated = DateTime.Now.ToString()
            });
            //var dapr = _httpClientFactory.CreateClient("dapr");
            //var msg = new HttpRequestMessage(HttpMethod.Get, "/v1.0/invoke/");
            return Ok();
        }
    }
}