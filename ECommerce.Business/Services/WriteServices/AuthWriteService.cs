﻿using ECommerce.Business.Extensions;
using ECommerce.Business.Helpers.TokenServices;
using ECommerce.Business.Models.Dtos.Auth;
using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers.PasswordServices;
using ECommerce.Entity.Entities;
using ECommerce.Entity.Enums;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Business.Services.WriteServices
{
    public class AuthWriteService : IAuthWriteService
    {
        private readonly IUserReadService _userReadService;
        private readonly IUserWriteService _userWriteService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<RegisterDto> _registerDtoValidator;

        public AuthWriteService(IUserReadService userReadService, IUserWriteService userWriteService,
            ITokenService tokenService, IPasswordGenerationService passwordGenerationService,
            IValidator<LoginDto> loginDtoValidator, IValidator<RegisterDto> registerDtoValidator)
        {
            _userReadService = userReadService;
            _userWriteService = userWriteService;
            _tokenService = tokenService;
            _passwordGenerationService = passwordGenerationService;
            _loginDtoValidator = loginDtoValidator;
            _registerDtoValidator = registerDtoValidator;
        }

        public async Task<string> UserLoginAsync(LoginDto loginUser)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(loginUser, _loginDtoValidator);

            var user = await _userReadService.Users.GetSingleAsync(_ => _.Mail == loginUser.Mail);
            if (user != null && _passwordGenerationService.VerifyPassword(user.PasswordSalt, user.Password, loginUser.Password))
            {
                await ActivateUserIfDeleted(user);
                var claims = ConfigureUserClaims(user);
                var token = _tokenService.GenerateToken(claims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            throw new BadRequestException("Hatalı mail adresi veya şifre!");
        }

        public async Task<bool> UserRegisterAsync(RegisterDto registerUser, Role role)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(registerUser, _registerDtoValidator);

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
                Company = IsCompany(registerUser, role),
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
                    new Claim(ClaimTypes.MobilePhone, user.Phone),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, Enum.GetName(user.Role))
            };

            return authClaims;
        }

        private async Task ActivateUserIfDeleted(User user)
        {
            if (!user.IsValid)
                await _userWriteService.ActivateUserAsync(user.Id.ToString());
        }
    }
}
