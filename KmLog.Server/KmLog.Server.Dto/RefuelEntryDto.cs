using System;

namespace KmLog.Server.Dto
{
    public class RefuelEntryDto : RefuelEntryInfoDto
    {
        public Guid CarId { get; set; }
        public CarInfoDto Car { get; set; }
    }
}
