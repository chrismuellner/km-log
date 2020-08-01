using System;

namespace KmLog.Server.Dto
{
    public class ServiceEntryDto : ServiceEntryInfoDto
    {
        public Guid CarId { get; set; }

        public CarInfoDto Car { get; set; }
    }
}
