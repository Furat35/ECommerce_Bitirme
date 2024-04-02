using ECommerce.Business.Models.Dtos.Auth;
using ECommerce.Entity.Enums;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IAuthWriteService
    {
        Task<bool> UserRegisterAsync(RegisterDto user, Role role);
        Task<string> UserLoginAsync(LoginDto user);
    }
}
