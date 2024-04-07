using ECommerce.Business.Models.Dtos.Auth;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Entity.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Authentication ve Authorization işlemlerinin yapılmasını sağlayan endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthWriteService _authWriteRepository;

        public AuthController(IAuthWriteService authWriteRepository)
        {
            _authWriteRepository = authWriteRepository;
        }

        /// <summary>
        /// Login işlemi
        /// </summary>
        /// <param name="user">Login olacak kullanıcı bilgileri</param>
        /// <returns>Giriş yapan kullanıcı için jwt Token</returns>
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var jwtToken = await _authWriteRepository.UserLoginAsync(user);
            return Ok(new { Token = jwtToken });
        }

        /// <summary>
        /// Kullanıcı üye olma
        /// </summary>
        /// <param name="user">Üye olacak kullanıcı bilgileri</param>
        /// <returns>İşlemin başarılı olma durumu</returns>
        [HttpPost("register/user", Name = "RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterDto user)
        {
            var registerSuccess = await _authWriteRepository.UserRegisterAsync(user, Role.User);
            return Ok(registerSuccess);
        }

        /// <summary>
        /// Şirket üye olma
        /// </summary>
        /// <param name="user">Üye olacak şirket bilgileri</param>
        /// <returns>İşlemin başarılı olma durumu</returns>
        [HttpPost("register/company", Name = "RegisterCompany")]
        public async Task<IActionResult> RegisterCompany(RegisterDto user)
        {
            var registerSuccess = await _authWriteRepository.UserRegisterAsync(user, Role.Company);
            return Ok(registerSuccess);
        }
    }
}
