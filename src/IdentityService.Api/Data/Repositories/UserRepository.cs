using IdentityService.Api.Data.Models;

namespace IdentityService.Api.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext dbcontext;

        public UserRepository(IdentityDbContext context)
        {
            dbcontext = context;
        }
        public async Task CreateUser(User user)
        {
           await dbcontext.Users.AddAsync(user);
           dbcontext.SaveChanges();
        }

        public async Task<User> GetUser(string username)
        {
             return await Task.Run(()=> dbcontext.Users.FirstOrDefault(user=> user.Username == username)); 
        }
    }
}
