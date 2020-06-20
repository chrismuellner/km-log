using KmLog.Server.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public partial class NavMenu
    {
        private bool _collapseNavMenu = true;

        private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        [Inject]
        private AppState State { get; set; }

        protected override void OnInitialized()
        {
            State.OnCarsChanged += StateHasChanged;
        }

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}
