using IdentityService.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
