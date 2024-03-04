using AutoMapper;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Helpers.Users;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Business.Services.Users.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.Users
{
    public class UserReadService : IUserReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            Users = unitOfWork.GetReadRepository<User>();
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IReadRepository<User> Users { get; }

        public async Task<UserListDto> GetUserByIdAsync(string userId)
        {
            var user = await Users.GetByIdAsync(userId);
            return _mapper.Map<UserListDto>(user);
        }

        public List<UserListDto> GetUsersWhere(UserRequestFilter filters, Expression<Func<User, bool>> predicate = null)
        {
            var users = predicate != null
                ? Users.GetWhere(predicate, includeProperties: [_ => _.Company])
                : Users.GetAll(includeProperties: [_ => _.Company]);
            var filteredProducts = new UserFilterService(_mapper, users).FilterUsers(filters);
            new HeaderService(_httpContextAccessor).AddToHeaders(filteredProducts.Headers);

            return filteredProducts.ResponseValue;
        }

        public async Task<bool> CheckIfUserExists(string email)
        {
            var user = await Users.GetSingleAsync(_ => _.Mail == email);
            return user != null ? true : false;
        }
    }
}
