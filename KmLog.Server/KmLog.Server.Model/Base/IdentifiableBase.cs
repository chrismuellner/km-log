using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmLog.Server.Model.Base
{
    public class IdentifiableBase
    {
        [Key, Column(Order = 0)]
        public Guid Id { get; set; }
    }
}
