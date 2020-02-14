using System;
using KmLog.Server.Dto.Base;

namespace KmLog.Server.Dto
{
    public class RefuelActionInfoDto : IdentifiableBaseDto
    {
        public DateTime Date { get; set; }
        public double TotalDistance { get; set; }
        public double Amount { get; set; }
        public double Cost { get; set; }
        public Guid CarId { get; set; }
    }
}
