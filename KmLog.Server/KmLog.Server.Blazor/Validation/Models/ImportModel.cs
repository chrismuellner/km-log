using KmLog.Server.Domain;

namespace KmLog.Server.Blazor.Validation.Models
{
    public class ImportModel
    {
        public EntryType EntryType { get; set; }

        public string DateColumn { get; set; }

        public string TotalDistanceColumn { get; set; }

        public string AmountColumn { get; set; }

        public string CostColumn { get; set; }

        public string PricePerLiterColumn { get; set; }

        public string TankStatusColumn { get; set; }

        public string LicensePlate { get; set; }

        public string ServiceTypeColumn { get; set; }
    }
}
