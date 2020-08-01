using System;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Validation.Models
{
    public class ServiceEntryModel : ServiceEntryDto, IEntryModel
    {
        public string CarIdAsString
        {
            get => CarId.ToString();
            set
            {
                if (Guid.TryParse(value, out var carGuid))
                    CarId = carGuid;
            }
        }

        public long? LatestTotalDistance { get; set; }
    }
}
