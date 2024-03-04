using ECommerce.Business.Models.Dtos.Auth;
using ECommerce.Entity.Enums;

namespace ECommerce.Business.Services.Authentication.Abstract
{
    public interface IAuthWriteService
    {
        Task<bool> UserRegisterAsync(RegisterDto user, Role role);
        Task<string> UserLoginAsync(LoginDto user);
    }
}
