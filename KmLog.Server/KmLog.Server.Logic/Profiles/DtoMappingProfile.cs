using AutoMapper;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Logic.Profiles
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<RefuelAction, RefuelActionDto>();
            CreateMap<Car, CarDto>();
        }
    }
}
