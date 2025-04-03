using AutoMapper;
using DAL.Entities;
using Service.DTOS;

namespace Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
