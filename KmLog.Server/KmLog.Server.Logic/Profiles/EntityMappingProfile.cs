using AutoMapper;
using KmLog.Server.DTO;
using KmLog.Server.Model;

namespace KmLog.Server.Logic.Profiles
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<JourneyDTO, Journey>();
        }
    }
}
