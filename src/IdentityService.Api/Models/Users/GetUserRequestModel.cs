using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Models.Users
{
    public class GetUserRequestModel
    {
        [FromHeader]
        public string? Authorization { get; set; }
    }
}
