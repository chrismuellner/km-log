using System;
using System.ComponentModel.DataAnnotations;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public abstract class Entry : IdentifiableBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public long TotalDistance { get; set; }

        public string Notes { get; set; }

        public Guid CarId { get; set; }
        public virtual Car Car { get; set; }
    }
}
