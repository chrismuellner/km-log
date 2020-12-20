using System;
using System.Net.Http;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Services;
using KmLog.Server.Blazor.Validation.Validators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

namespace KmLog.Server.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddApiAuthorization();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            // services
            builder.Services.AddSingleton<AppState>();
            builder.Services.AddScoped<ClipboardService>();

            // custom validators
            builder.Services.AddScoped<RefuelEntryValidator>();
            builder.Services.AddScoped<CarValidator>();
            builder.Services.AddScoped<ImportValidator>();
            builder.Services.AddScoped<JoinGroupValidator>();
            builder.Services.AddScoped<AddGroupValidator>();
            builder.Services.AddScoped<ServiceEntryValidator>();

            // blazorize
            builder.Services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            var host = builder.Build();

            host.Services
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}
