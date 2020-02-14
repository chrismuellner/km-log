using System.ComponentModel.DataAnnotations;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class User : IdentifiableBase
    {
        [Required]
        public string Email { get; set; }
    }
}
