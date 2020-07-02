using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Services;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddEntryComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private AppState State { get; set; }

        [Parameter]
        public string LicensePlate { get; set; }

        private CarInfoDto ActiveCar { get; set; }

        private RefuelEntryInfoDto LatestRefuelEntry { get; set; }

        private RefuelEntryModel RefuelEntry { get; set; } = new RefuelEntryModel
        {
            Date = DateTime.Today
        };

        protected override async Task OnParametersSetAsync()
        {
            if (State.Cars == null || !State.Cars.Any())
            {
                return;
            }

            ActiveCar = LicensePlate == null
                ? State.Cars.First()
                : State.Cars.First(c => c.LicensePlate == LicensePlate);
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

            await HttpClient.PutAsJsonAsync("api/entry/refuel", RefuelEntry);
        }

        private async Task UpdateCarId(string carId)
        {
            RefuelEntry.CarIdAsString = carId;
            ActiveCar = State.Cars.First(c => c.Id == RefuelEntry.CarId);

            await LoadLatestRefuelEntry();
        }

        private async Task LoadLatestRefuelEntry()
        {
            try
            {
                LatestRefuelEntry = await HttpClient.GetFromJsonAsync<RefuelEntryInfoDto>($"api/entry/refuel/{ActiveCar.LicensePlate}/latest");
                RefuelEntry.LatestTotalDistance = LatestRefuelEntry.TotalDistance;
                RefuelEntry.PricePerLiter = LatestRefuelEntry.PricePerLiter;

                if (RefuelEntry.TotalDistance == 0)
                {
                    RefuelEntry.TotalDistance = LatestRefuelEntry.TotalDistance;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("No latest refuelentry exists for current car");
                RefuelEntry.LatestTotalDistance = default;
            }
        }
    }
}
