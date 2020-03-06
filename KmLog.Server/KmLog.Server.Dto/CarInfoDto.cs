using KmLog.Server.Domain;
using KmLog.Server.Dto.Base;

namespace KmLog.Server.Dto
{
    public class CarInfoDto : IdentifiableBaseDto
    {
        public string LicensePlate { get; set; }

        public FuelType FuelType { get; set; }

        public long InitialDistance { get; set; }

        public string Description { get; set; }
    }
}
