using System.Text.Json.Serialization;
using KmLog.Server.Dal.DI;
using KmLog.Server.EF.DI;
using KmLog.Server.Logic.DI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KmLog.Server.WebApi
{
    public class Startup
    {
        private const string AzureSection = "Azure";
        private const string ClientIdKey = "ClientId";
        private const string ClientSecretKey = "ClientSecret";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opts =>
            {
                opts.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opts.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddMicrosoftAccount(opts =>
            {
                var azureSection = Configuration.GetSection(AzureSection);

                opts.ClientId = azureSection[ClientIdKey];
                opts.ClientSecret = azureSection[ClientSecretKey];
                //opts.ReturnUrlParameter = "http://localhost:4141/Authentication/signin-microsoft";
            });

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddDbContext(Configuration);
            services.AddAutoMapper();
            services.AddRepositories();
            services.AddLogic();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(cfg => cfg.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
