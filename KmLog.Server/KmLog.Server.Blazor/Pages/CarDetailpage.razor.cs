using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Domain;
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

        private PagingParameters PagingParams { get; } = new PagingParameters { PageIndex = 0, PageSize = 10 };

        private PagingResult<RefuelEntryInfoDto> RefuelEntries { get; set; }

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

            try
            {
                RefuelEntries = await HttpClient.GetFromJsonAsync<PagingResult<RefuelEntryInfoDto>>($"api/entry/refuel/{LicensePlate}");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Could not parse json for refuel entries!");
            }
        }

        private async Task PagingChanged(int page)
        {
            PagingParams.PageIndex = page;
            RefuelEntries = await HttpClient.GetFromJsonAsync<PagingResult<RefuelEntryInfoDto>>(
                $"api/entry/refuel/{LicensePlate}?PageIndex={PagingParams.PageIndex}&PageSize={PagingParams.PageSize}");
        }
    }
}
