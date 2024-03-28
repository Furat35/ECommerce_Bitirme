using ECommerce.Business.Helpers.TokenServices;
using ECommerce.Business.Models.Dtos.Auth;
using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Business.Services.Authentication.Abstract;
using ECommerce.Business.Services.Users.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers.PasswordServices;
using ECommerce.Entity.Entities;
using ECommerce.Entity.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Business.Services.Authentication
{
    public class AuthWriteService : IAuthWriteService
    {
        private readonly IUserReadService _userReadService;
        private readonly IUserWriteService _userWriteService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordGenerationService _passwordGenerationService;

        public AuthWriteService(IUserReadService userReadService, IUserWriteService userWriteService,
            ITokenService tokenService, IPasswordGenerationService passwordGenerationService)
        {
            _userReadService = userReadService;
            _userWriteService = userWriteService;
            _tokenService = tokenService;
            _passwordGenerationService = passwordGenerationService;
        }

        public async Task<string> UserLoginAsync(LoginDto loginUser)
        {
            var user = await _userReadService.Users.GetSingleAsync(_ => _.Mail == loginUser.Mail);
            if (user != null && _passwordGenerationService.VerifyPassword(user.PasswordSalt, user.Password, loginUser.Password))
            {
                await ActiveUserIfDeleted(user);
                var claims = ConfigureUserClaims(user);
                var token = _tokenService.GenerateToken(claims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            throw new BadRequestException("Hatalı mail adresi veya şifre!");
        }

        public async Task<bool> UserRegisterAsync(RegisterDto registerUser, Role role)
        {
            if (await _userReadService.CheckIfUserExists(registerUser.Mail))
                throw new BadRequestException("Mail adresi kayıtlı! Başka mail adresi kullanın.");

            var salt = _passwordGenerationService.GenerateSalt();
            var combinedBytes = _passwordGenerationService.Combine(Encoding.UTF8.GetBytes(registerUser.Password), salt);
            var hashedBytes = _passwordGenerationService.HashBytes(combinedBytes);
            string storedSalt = Convert.ToBase64String(salt);
            string storedHashedPassword = Convert.ToBase64String(hashedBytes);

            var user = new UserAddDto
            {
                Name = registerUser.Name,
                Surname = registerUser.Surname,
                Mail = registerUser.Mail,
                Password = storedHashedPassword,
                PasswordSalt = storedSalt,
                Phone = registerUser.Phone,
                Role = role,
                Company = IsCompany(registerUser, role)
            };
            bool result = await _userWriteService.AddUserAsync(user);

            return result;
        }

        private CompanyAddDto IsCompany(RegisterDto registerUser, Role role)
        {
            if (role == Role.Company)
            {
                if (registerUser.Company is null)
                    throw new BadRequestException("Şirket bilgileriniz giriniz!");

                return new CompanyAddDto
                {
                    CompanyName = registerUser.Company.CompanyName,
                    Phone = registerUser.Company.Phone,
                    Mail = registerUser.Company.Mail,
                    AboutCompany = registerUser.Company.AboutCompany
                };
            }

            return null;
        }


        private List<Claim> ConfigureUserClaims(User user)
        {
            var authClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                    new Claim(ClaimTypes.Email, user.Mail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, Enum.GetName(user.Role))
            };

            return authClaims;
        }

        private async Task ActiveUserIfDeleted(User user)
        {
            if (!user.IsValid)
                await _userWriteService.ActivateUserAsync(user.Id.ToString());
        }
    }
}
