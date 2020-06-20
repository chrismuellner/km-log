using System;
using System.Net.Http;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Services;
using KmLog.Server.Blazor.Validation.Validators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            builder.Services.AddApiAuthorization();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            // services
            builder.Services.AddSingleton<AppState>();

            // custom validators
            builder.Services.AddScoped<RefuelEntryValidator>();
            builder.Services.AddScoped<CarValidator>();
            builder.Services.AddScoped<ImportValidator>();
            builder.Services.AddScoped<JoinGroupValidator>();
            builder.Services.AddScoped<AddGroupValidator>();

            await builder.Build().RunAsync();
        }
    }
}
