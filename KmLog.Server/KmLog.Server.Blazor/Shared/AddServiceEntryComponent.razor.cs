using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddServiceEntryComponent : AddEntryBase<ServiceEntryModel>
    {
        protected override ServiceEntryModel Entry { get; set; } = new ServiceEntryModel
        {
            Date = DateTime.Today
        };

        private ServiceEntryInfoDto LatestServiceEntry { get; set; }

        protected override async Task FormSubmitted()
        {
            try
            {
                await HttpClient.PutAsJsonAsync("api/entry/service", Entry);
                Entry = new ServiceEntryModel
                {
                    Date = DateTime.Today
                };
            }
            catch (Exception)
            {
                Debug.WriteLine("Error saving servic entry");
            }
        }

        protected override async Task LoadLatestEntry()
        {
            try
            {
                LatestServiceEntry = await HttpClient.GetFromJsonAsync<ServiceEntryInfoDto>($"api/entry/service/{ActiveCar.LicensePlate}/latest");
                Entry.LatestTotalDistance = LatestServiceEntry.TotalDistance;

                if (Entry.TotalDistance == 0)
                {
                    Entry.TotalDistance = LatestServiceEntry.TotalDistance;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("No latest service entry exists for current car");
                Entry.LatestTotalDistance = default;
            }
        }
    }
}
