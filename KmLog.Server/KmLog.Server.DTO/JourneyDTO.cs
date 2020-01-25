using System;
using KmLog.Server.DTO.Base;

namespace KmLog.Server.DTO
{
    public class JourneyDTO : IdentifiableBaseDTO
    {
        public DateTime Date { get; set; }
        public double Distance { get; set; }
    }
}
