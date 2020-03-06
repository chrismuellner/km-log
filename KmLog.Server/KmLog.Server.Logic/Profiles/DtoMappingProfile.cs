﻿using AutoMapper;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Logic.Profiles
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<RefuelEntry, RefuelEntryDto>();
            CreateMap<RefuelEntry, RefuelEntryInfoDto>();
            CreateMap<Car, CarDto>();
            CreateMap<Car, CarInfoDto>();
            CreateMap<User, UserDto>();
        }
    }
}
