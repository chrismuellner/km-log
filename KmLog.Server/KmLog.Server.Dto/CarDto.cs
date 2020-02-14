using System.Collections.Generic;

namespace KmLog.Server.Dto
{
    public class CarDto : CarInfoDto
    {
        public ICollection<RefuelActionInfoDto> RefuelActions { get; set; }
        public UserDto User { get; set; }
    }
}
