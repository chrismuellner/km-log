using System.ComponentModel.DataAnnotations;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class Group : IdentifiableBase
    {
        [Required]
        public string Name { get; set; }
    }
}
