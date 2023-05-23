using System;
using AutoMapper;
using logistics.Dtos;
using logistics.Models;

namespace logistics.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Admin, AdminDto>();
            CreateMap<AdminDto, Admin>();
        }
    }
}

