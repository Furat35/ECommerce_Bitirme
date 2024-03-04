using ECommerce.Business.Helpers.Users;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.Users.Abstract
{
    public interface IUserReadService
    {
        Task<UserListDto> GetUserByIdAsync(string userId);
        Task<bool> CheckIfUserExists(string email);
        IReadRepository<User> Users { get; }
        List<UserListDto> GetUsersWhere(UserRequestFilter filters, Expression<Func<User, bool>> predicate = null);
    }
}
