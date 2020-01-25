using System.Collections.Generic;
using KmLog.Server.Domain;
using KmLog.Server.Dto.Base;

namespace KmLog.Server.Dto
{
    public class CarDto : IdentifiableBaseDto
    {
        public string LicensePlate { get; set; }
        public FuelType FuelType { get; set; }

        public ICollection<RefuelActionDto> RefuelActions { get; set; }
    }
}
