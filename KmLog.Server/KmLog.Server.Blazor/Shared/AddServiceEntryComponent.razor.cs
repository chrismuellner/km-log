using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddServiceEntryComponent : AddEntryBase
    {
        protected override IEntryModel Entry { get; set; } = new ServiceEntryModel
        {
            Date = DateTime.Today
        };

        private ServiceEntryModel ServiceEntry
        {
            get => Entry as ServiceEntryModel;
            set => Entry = value;
        }

        private ServiceEntryInfoDto LatestServiceEntry { get; set; }

        protected override async Task FormSubmitted()
        {
            try
            {
                await HttpClient.PutAsJsonAsync("api/entry/service", ServiceEntry);
                ServiceEntry = new ServiceEntryModel
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
                ServiceEntry.LatestTotalDistance = LatestServiceEntry.TotalDistance;

                if (ServiceEntry.TotalDistance == 0)
                {
                    ServiceEntry.TotalDistance = LatestServiceEntry.TotalDistance;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("No latest service entry exists for current car");
                ServiceEntry.LatestTotalDistance = default;
            }
        }
    }
}
