using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KmLog.Server.Blazor.Pages
{
    [Authorize]
    public partial class CarDetailPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public string LicensePlate { get; set; }

        private CarStatisticDto CarStatistic { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            if (!authState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"api/authentication/signin", true);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                CarStatistic = await HttpClient.GetFromJsonAsync<CarStatisticDto>($"api/car/{LicensePlate}");
            }
            catch (Exception)
            {
                CarStatistic = null;
                Console.Error.WriteLine("Could not parse json for car statistic!");
            }
        }
    }
}
