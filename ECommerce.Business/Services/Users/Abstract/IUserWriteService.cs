using ECommerce.Business.Models.Dtos.Users;

namespace ECommerce.Business.Services.Users.Abstract
{
    public interface IUserWriteService
    {
        Task<bool> UpdateUserPasswordAsync(string password);
        Task<bool> SafeDeleteUserAsync(string userId);
        Task<bool> AddUserAsync(UserAddDto user);
        Task<bool> ActivateUserAsync(string userId);
    }
}
