using AutoMapper;
using KmLog.Server.DTO;
using KmLog.Server.Model;

namespace KmLog.Server.Logic.Profiles
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Journey, JourneyDTO>();
        }
    }
}
