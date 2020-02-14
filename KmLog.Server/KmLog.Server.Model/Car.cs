using System;
using System.Collections.Generic;
using KmLog.Server.Domain;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class Car : IdentifiableBase
    {
        public string LicensePlate { get; set; }
        public FuelType FuelType { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RefuelAction> RefuelActions { get; set; }
    }
}
