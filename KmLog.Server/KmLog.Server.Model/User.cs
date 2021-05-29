using System;
using KmLog.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace KmLog.Server.Model
{
    public class User : IdentityUser
    {
        public UserRole? Role { get; set; }

        public Guid? GroupId { get; set; }

        public Group Group { get; set; }
    }
}
