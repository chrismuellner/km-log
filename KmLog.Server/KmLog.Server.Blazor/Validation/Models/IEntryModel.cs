using System;

namespace KmLog.Server.Blazor.Validation.Models
{
    public interface IEntryModel
    {
        Guid CarId { get; set; }

        string CarIdAsString { get; set; }

        long? LatestTotalDistance { get; set; }
    }
}
