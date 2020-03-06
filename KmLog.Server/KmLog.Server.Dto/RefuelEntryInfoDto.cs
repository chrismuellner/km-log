using KmLog.Server.Domain;

namespace KmLog.Server.Dto
{
    public class RefuelEntryInfoDto : EntryDto
    {
        public long Distance { get; set; }

        public long TotalDistance { get; set; }

        public double Amount { get; set; }

        public double PricePerLiter { get; set; }

        public TankStatus TankStatus { get; set; }
    }
}
