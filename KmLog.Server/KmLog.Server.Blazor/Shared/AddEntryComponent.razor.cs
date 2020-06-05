using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddEntryComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Parameter]
        public string LicensePlate { get; set; }

        [CascadingParameter]
        private IEnumerable<CarInfoDto> Cars { get; set; }

        private CarInfoDto ActiveCar { get; set; }

        private RefuelEntryInfoDto LatestRefuelEntry { get; set; }

        private RefuelEntryModel RefuelEntry { get; set; } = new RefuelEntryModel
        {
            Date = DateTime.Today
        };

        protected override async Task OnParametersSetAsync()
        {
            ActiveCar = LicensePlate == null
                ? Cars.First()
                : Cars.First(c => c.LicensePlate == LicensePlate);
            RefuelEntry.CarId = ActiveCar.Id;

            await LoadLatestRefuelEntry();
        }

        private async Task FormSubmitted()
        {
            if (RefuelEntry.Distance == 0)
            {
                RefuelEntry.Distance = RefuelEntry.TotalDistance - RefuelEntry.LatestTotalDistance.Value;
            }
            if (RefuelEntry.TotalDistance == 0)
            {
                RefuelEntry.TotalDistance = RefuelEntry.LatestTotalDistance.Value + RefuelEntry.Distance;
            }

            await HttpClient.PutAsJsonAsync("api/refuelentry", RefuelEntry);
        }

        private async Task UpdateCarId(string carId)
        {
            RefuelEntry.CarIdAsString = carId;
            ActiveCar = Cars.First(c => c.Id == RefuelEntry.CarId);

            await LoadLatestRefuelEntry();
        }

        private async Task LoadLatestRefuelEntry()
        {
            try
            {
                LatestRefuelEntry = await HttpClient.GetFromJsonAsync<RefuelEntryInfoDto>($"api/refuelentry/{ActiveCar.LicensePlate}/latest");
                RefuelEntry.LatestTotalDistance = LatestRefuelEntry.TotalDistance;
                RefuelEntry.PricePerLiter = LatestRefuelEntry.PricePerLiter;

                if (RefuelEntry.TotalDistance == 0)
                {
                    RefuelEntry.TotalDistance = LatestRefuelEntry.TotalDistance;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No latest refuelentry exists for current car");
                RefuelEntry.LatestTotalDistance = default;
            }
        }
    }
}
