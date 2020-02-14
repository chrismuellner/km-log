using AutoMapper;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Logic.Profiles
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<RefuelActionDto, RefuelAction>();
            CreateMap<RefuelActionInfoDto, RefuelAction>();
            CreateMap<CarDto, Car>();
            CreateMap<CarInfoDto, Car>();
            CreateMap<UserDto, User>();
        }
    }
}
