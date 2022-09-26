using System.ComponentModel.DataAnnotations;

namespace IdentityService.Api.Models.Users
{
    public class GetUserResponseModel
    {
        public string? Username { get; set; }
        public string? Mobile { get; set; }
        public DateTime Registered { get; set; }
    }
}
