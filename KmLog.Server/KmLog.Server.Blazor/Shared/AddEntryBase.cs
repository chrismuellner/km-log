using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Services;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public abstract class AddEntryBase<T> : ComponentBase
        where T : IEntryModel
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected AppState State { get; set; }

        [Parameter]
        public string LicensePlate { get; set; }

        protected CarInfoDto ActiveCar { get; set; }

        protected abstract T Entry { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (State.Cars == null || !State.Cars.Any())
                {
                    return;
                }

                ActiveCar = LicensePlate == null
                    ? State.Cars.First()
                    : State.Cars.First(c => c.LicensePlate == LicensePlate);
                Entry.CarId = ActiveCar.Id;

                await LoadLatestEntry();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error setting parameters for AddEntry");
                throw;
            }
        }

        protected async Task UpdateCarId(string carId)
        {
            Entry.CarIdAsString = carId;
            ActiveCar = State.Cars.First(c => c.Id == Entry.CarId);

            await LoadLatestEntry();
        }

        protected abstract Task FormSubmitted();

        protected abstract Task LoadLatestEntry();
    }
}
