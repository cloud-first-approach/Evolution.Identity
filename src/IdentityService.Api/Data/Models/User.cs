using System.ComponentModel.DataAnnotations;

namespace IdentityService.Api.Data.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Salt { get; set; }
        public string? Mobile { get; set; }
        public bool IsActive { get; set; }
        public DateTime Registered { get; set; }
    }
}
