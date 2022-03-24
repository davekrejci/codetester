using AutoMapper;
using Codetester.Dtos;
using Codetester.Models;

namespace Codetester.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<User, UserLoginReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserLoginDto, User>();
        }
    }
}