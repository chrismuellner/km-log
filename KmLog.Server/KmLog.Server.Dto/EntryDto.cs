using System;
using KmLog.Server.Dto.Base;

namespace KmLog.Server.Dto
{
    public abstract class EntryDto : IdentifiableBaseDto
    {
        public DateTime Date { get; set; }

        public double Cost { get; set; }

        public long Distance { get; set; }

        public long TotalDistance { get; set; }

        public string Notes { get; set; }
    }
}
