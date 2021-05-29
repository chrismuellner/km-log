using System;
using System.Collections.Generic;

namespace KmLog.Server.Dto
{
    public class CarDto : CarInfoDto
    {
        public ICollection<RefuelEntryInfoDto> RefuelEntries { get; set; }

        public string UserId { get; set; }

        public UserDto User { get; set; }
    }
}
