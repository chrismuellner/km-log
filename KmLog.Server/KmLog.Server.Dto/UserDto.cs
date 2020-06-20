using System;
using KmLog.Server.Domain;
using KmLog.Server.Dto.Base;

namespace KmLog.Server.Dto
{
    public class UserDto : IdentifiableBaseDto
    {
        public string Email { get; set; }

        public UserRole? Role { get; set; }

        public Guid? GroupId { get; set; }

        public GroupDto Group { get; set; }
    }
}
