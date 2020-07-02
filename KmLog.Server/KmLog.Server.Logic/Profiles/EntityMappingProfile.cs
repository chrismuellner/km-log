using AutoMapper;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Logic.Profiles
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<RefuelEntryDto, RefuelEntry>();
            CreateMap<RefuelEntryInfoDto, RefuelEntry>();
            CreateMap<ServiceEntryDto, ServiceEntry>();
            CreateMap<ServiceEntryInfoDto, ServiceEntry>();
            CreateMap<CarDto, Car>();
            CreateMap<CarInfoDto, Car>();
            CreateMap<UserDto, User>();
            CreateMap<GroupDto, Group>();
        }
    }
}
