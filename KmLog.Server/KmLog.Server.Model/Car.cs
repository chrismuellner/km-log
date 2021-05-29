using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KmLog.Server.Domain;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class Car : IdentifiableBase
    {
        [Required]
        public string LicensePlate { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public long InitialDistance { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RefuelEntry> RefuelEntries { get; set; }
    }
}
