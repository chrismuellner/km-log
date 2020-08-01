using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddRefuelEntryComponent : AddEntryBase
    {
        protected override IEntryModel Entry { get; set; } = new RefuelEntryModel
        {
            Date = DateTime.Today
        };

        private RefuelEntryModel RefuelEntry
        {
            get => Entry as RefuelEntryModel;
            set => Entry = value;
        }

        private RefuelEntryInfoDto LatestRefuelEntry { get; set; }

        protected override async Task FormSubmitted()
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

        protected override async Task LoadLatestEntry()
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
                Debug.WriteLine("No latest refuel entry exists for current car");
                RefuelEntry.LatestTotalDistance = default;
            }
        }
    }
}
