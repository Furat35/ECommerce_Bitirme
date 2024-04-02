using ECommerce.Business.ActionFilters;
using ECommerce.Business.Helpers.Users;
using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Business.Validations.FluentValidations.Addresses;
using ECommerce.Core.Consts;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Kullanıcı crud işlemleri ile ilgili endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserReadService _userReadService;
        private readonly IUserWriteService _userWriteService;

        public UsersController(IUserReadService userReadService, IUserWriteService userWriteService)
        {
            _userReadService = userReadService;
            _userWriteService = userWriteService;
        }

        /// <summary>
        /// Kullanıcılar listeleniyor.
        /// </summary>
        /// <param name="userRequestFilter">Kullanıcı filterleri</param>
        /// <returns>Kullanıcılar getirilmektedir.</returns>
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = $"{RoleConsts.Admin}")]
        public IActionResult GetUsers([FromQuery] UserRequestFilter userRequestFilter)
        {
            var users = _userReadService.GetUsersWhere(userRequestFilter, _ => _.IsValid == userRequestFilter.IsValid);
            return Ok(users);
        }

        /// <summary>
        /// Verilen id'deki kullanıcı getirilmektedir.
        /// </summary>
        /// <param name="id">Getirilecek kullanıcının id'si</param>
        /// <returns>kullanıcı bilgileri</returns>
        [HttpGet("{id}", Name = "GetByUserId")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company},{RoleConsts.User}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (User.GetActiveUserId().Equals(id, StringComparison.InvariantCultureIgnoreCase) || User.IsInRole(RoleConsts.Admin))
            {
                var user = await _userReadService.GetUserByIdAsync(id);
                return Ok(user);
            }

            throw new ForbiddenException();
        }

        /// <summary>
        /// Kullanıcı silme
        /// </summary>
        /// <param name="id">Silinecek olan kullanının id'si</param>
        /// <returns>Ok</returns>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company},{RoleConsts.User}")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            if (User.GetActiveUserId().Equals(id, StringComparison.InvariantCultureIgnoreCase)
                || User.IsInRole(RoleConsts.Admin))
            {
                var result = await _userWriteService.SafeDeleteUserAsync(id);
                return Ok(result);
            }

            throw new ForbiddenException();
        }

        /// <summary>
        /// Şifre güncelleme
        /// </summary>
        /// <param name="password">Yeni şifre</param>
        /// <returns>İşlem başarılı olup olmadığı</returns>
        [HttpPut("password", Name = "UpdatePassword")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["password"])]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company},{RoleConsts.User}")]
        public async Task<IActionResult> UpdatePassword([FromBody] string password)
        {
            var updateResult = await _userWriteService.UpdateUserPasswordAsync(password);
            return Ok(updateResult);
        }

        /// <summary>
        /// Adres güncelleme
        /// </summary>
        /// <param name="address">Yeni adres</param>
        /// <returns>İşlemin başarılı olup olmadığı</returns>
        [HttpPut("address")]
        [TypeFilter(typeof(FluentValidationFilterAttribute<AddressUpdateDtoValidator, AddressUpdateDto>), Arguments = ["address"])]
        [Authorize(Roles = $"{RoleConsts.Company},{RoleConsts.User}")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressUpdateDto address)
        {
            var updateResult = await _userWriteService.UpdateAddress(address);
            return Ok(updateResult);
        }

        /// <summary>
        /// Kullanıcı aktifleştirme
        /// </summary>
        /// <param name="id">Kullanıcı id'si</param>
        /// <returns>İşlemin başarılı olup olmadığı</returns>
        [HttpPut("{id}", Name = "ActivateUser")]
        [TypeFilter(typeof(ModelValidationFilterAttribute), Arguments = ["id"])]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company},{RoleConsts.User}")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            if (User.GetActiveUserId().Equals(id, StringComparison.InvariantCultureIgnoreCase)
              || User.IsInRole(RoleConsts.Admin))
            {
                var updateResult = await _userWriteService.ActivateUserAsync(id);
                return Ok(updateResult);
            }

            throw new ForbiddenException();
        }
    }
}
