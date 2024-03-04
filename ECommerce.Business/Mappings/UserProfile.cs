using AutoMapper;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
            CreateMap<User, UserListDto>();
        }
    }
}
