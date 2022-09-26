using IdentityService.Api.Data;
using IdentityService.Api.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace IdentityService.Api
{
    public static class PrepareDbInitial
    {
        public static void PrePoulateData(IApplicationBuilder application, IWebHostEnvironment env)
        {
            using (var serviceRepo = application.ApplicationServices.CreateScope())
            {
                SeedData(serviceRepo.ServiceProvider.GetService<IdentityDbContext>(), env);
            }
        }

        private static void SeedData(
            IdentityDbContext identityDbContext,
            IWebHostEnvironment env
        )
        {
            System.Console.WriteLine("Environment Variables");
            System.Console.WriteLine(env.IsProduction().ToString());
            if (env.IsProduction())
            {
                System.Console.WriteLine("Applying Migrations Inside.");
                identityDbContext.Database.Migrate();
            }

            if (!identityDbContext.Users.Any())
            {
                Console.WriteLine("Seeding Data.....");
                identityDbContext.AddRange(
                    new User()
                    {
                        Username = "sourabh",
                        Password = "sourabh",
                        Id = 1,
                    }
                );
                identityDbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Data already Present");
            }
        }
    }
}
