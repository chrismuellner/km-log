using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddRefuelEntryComponent : AddEntryBase<RefuelEntryModel>
    {
        private bool collapseVisible = false;

        protected override RefuelEntryModel Entry { get; set; } = new RefuelEntryModel
        {
            Date = DateTime.Today
        };

        private RefuelEntryInfoDto LatestRefuelEntry { get; set; }

        protected override async Task FormSubmitted()
        {
            if (Entry.Distance == 0)
            {
                Entry.Distance = Entry.TotalDistance - Entry.LatestTotalDistance.Value;
            }
            if (Entry.TotalDistance == 0)
            {
                Entry.TotalDistance = Entry.LatestTotalDistance.Value + Entry.Distance;
            }

            await HttpClient.PutAsJsonAsync("api/entry/refuel", Entry);
        }

        protected override async Task LoadLatestEntry()
        {
            try
            {
                LatestRefuelEntry = await HttpClient.GetFromJsonAsync<RefuelEntryInfoDto>($"api/entry/refuel/{ActiveCar.LicensePlate}/latest");
                Entry.LatestTotalDistance = LatestRefuelEntry.TotalDistance;
                Entry.PricePerLiter = LatestRefuelEntry.PricePerLiter;

                if (Entry.TotalDistance == 0)
                {
                    Entry.TotalDistance = LatestRefuelEntry.TotalDistance;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("No latest refuel entry exists for current car");
                Entry.LatestTotalDistance = default;
            }
        }
    }
}
