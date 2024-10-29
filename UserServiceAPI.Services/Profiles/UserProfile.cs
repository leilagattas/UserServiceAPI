using AutoMapper;
using UserServiceAPI.Domain.Models;
using UserServiceAPI.Services.DTO;

namespace UserServiceAPI.Services.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
        }
    }
}
