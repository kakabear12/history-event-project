using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;

namespace WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<User, CurrentUserResponse>();
        }
    }
}
