using Duende.IdentityServer.Models;
using IdentityService.Api.AppSettings;
using IdentityService.Api.Data;
using IdentityService.Api.Data.Repositories;
using IdentityService.Api.Middlewares;
using IdentityService.Api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        public static IEnumerable<ApiScope> ApiScopes =>
    new List<ApiScope>
    {
        new ApiScope(name: "api1", displayName: "MyAPI")
    };
        public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "client",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "api1" }
        }
    };
        public IConfiguration Configuration { get; }

        private IWebHostEnvironment _env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Log.Information("using IdentityDB");
                services.AddDbContext<IdentityDbContext>(
                    optionsAction: options =>
                        options.UseSqlServer(Configuration.GetConnectionString("IdentityDB"))
                );
            }
            else
            {
                Log.Information("using InMemDB");
                services.AddDbContext<IdentityDbContext>(
                    optionsAction: options => options.UseInMemoryDatabase("InMemDB")
                );
            }

            services.AddIdentityServer()
           .AddInMemoryApiScopes(ApiScopes)
           .AddInMemoryClients(Clients);

            services.AddOptions<AuthSettings>().BindConfiguration("AuthSettings");

            //var authset = new AuthSettings();
            //Configuration.GetSection("AuthSettings").Bind(authset);

            services.AddScoped<ILoginStateRepository, LoginStateRepository>();
            services.AddScoped<IAuthManagerService, AuthManagerService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserManager, UserManager>();

            services.AddJwtAuthentication(Configuration);
            services.AddHttpClient();
            services.AddSwaggerGen();
            services.AddControllers().AddDapr();
            services.AddEndpointsApiExplorer();
            services.AddControllersWithViews();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseJwtParser();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "Admin",
                        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );

                    endpoints.MapDefaultControllerRoute();
                }
            );

            PrepareDb.PrePoulateData(app, env);
        }
    }
}
