using System;

namespace KmLog.Server.Dto
{
    public class ServiceEntryDto : EntryDto
    {
        public Guid CarId { get; set; }

        public CarInfoDto Car { get; set; }
    }
}
