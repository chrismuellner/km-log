using System;
using System.ComponentModel.DataAnnotations;
using KmLog.Server.Domain;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Model
{
    public class User : IdentifiableBase
    {
        [Required]
        public string Email { get; set; }

        public UserRole? Role { get; set; }

        public Guid? GroupId { get; set; }

        public Group Group { get; set; }
    }
}
