using System;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KmLog.Server.Blazor.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private AppState State { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            if (authState.User.Identity.IsAuthenticated)
            {
                try
                {
                    await State.UpdateCars();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}
