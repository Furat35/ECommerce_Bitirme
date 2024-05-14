using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IUserWriteService
    {
        Task<bool> UpdateUserPasswordAsync(string password);
        Task<bool> UpdateAddress(AddressUpdateDto addressUpdateDto);
        Task<bool> UpdateUser(UserUpdateDto userUpdateDto, string userId);
        Task<bool> SafeDeleteUserAsync(string userId);
        Task<bool> AddUserAsync(UserAddDto user);
        Task<bool> ActivateUserAsync(string userId);
        IWriteRepository<User> Users { get; set; }
    }
}
