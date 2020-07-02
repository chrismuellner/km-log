using KmLog.Server.Domain;

namespace KmLog.Server.Dto
{
    public class RefuelEntryInfoDto : EntryDto
    {
        public double Amount { get; set; }

        public double PricePerLiter { get; set; }

        public TankStatus TankStatus { get; set; }
    }
}
