using System;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class RefuelAction : IdentifiableBase
    {
        public DateTime Date { get; set; }
        public double TotalDistance { get; set; }
        public double Amount { get; set; }
        public double Cost { get; set; }

        public Guid CarId { get; set; }
        public virtual Car Car { get; set; }
    }
}
