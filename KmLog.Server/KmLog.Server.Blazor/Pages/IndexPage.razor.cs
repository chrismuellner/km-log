using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KmLog.Server.Blazor.Pages
{
    public partial class IndexPage
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            if (!authState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("api/authentication/signin?redirectUri=/", true);
            }
        }
    }
}
