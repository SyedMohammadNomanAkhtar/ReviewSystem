using AutoMapper;
using ReviewSystem.DTOs;
using ReviewSystem.Models;

namespace ReviewSystem.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UserDtoCreate, User>();
            CreateMap<ProductDtoCreate, Product>();
            CreateMap<ReviewDtoCreate, Review>();
        }
    }
}
