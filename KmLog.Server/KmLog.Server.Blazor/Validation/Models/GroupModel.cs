using System;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Validation.Models
{
    public class GroupModel : GroupDto
    {
        public string IdAsString
        {
            get => Id.ToString();
            set
            {
                if (Guid.TryParse(value, out var guid))
                    Id = guid;
            }
        }
    }
}
