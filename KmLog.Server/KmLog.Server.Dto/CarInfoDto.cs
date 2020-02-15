using System;
using KmLog.Server.Domain;
using KmLog.Server.Dto.Base;

namespace KmLog.Server.Dto
{
    public class CarInfoDto : IdentifiableBaseDto
    {
        public string LicensePlate { get; set; }
        public FuelType FuelType { get; set; }

        public Guid UserId { get; set; }

    }
}
