using System;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class Journey : IdentifiableBase
    {
        public DateTime Date { get; set; }
        public double Distance { get; set; }
    }
}
