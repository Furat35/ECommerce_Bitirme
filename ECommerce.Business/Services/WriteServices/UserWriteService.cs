using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using ECommerce.Core.Helpers.PasswordServices;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using ECommerce.Entity.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ECommerce.Business.Services.WriteServices
{
    public class UserWriteService : IUserWriteService
    {
        private readonly IMapper _mapper;
        private readonly IWriteRepository<User> _userWriteRepository;
        private readonly IUserReadService _userReadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IValidator<UserAddDto> _userAddDtoValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateDtoValidator;
        private readonly IValidator<AddressUpdateDto> _addressUpdateDtoValidator;

        public UserWriteService(IMapper mapper, IUnitOfWork unitOfWork, IUserReadService userReadService,
            IHttpContextAccessor httpContextAccessor, IPasswordGenerationService passwordGenerationService,
            IValidator<UserAddDto> userAddDtoValidator, IValidator<AddressUpdateDto> addressUpdateDtoValidator)
        {
            _userWriteRepository = unitOfWork.GetWriteRepository<User>();
            Users = _userWriteRepository;
            _userReadService = userReadService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _passwordGenerationService = passwordGenerationService;
            _userAddDtoValidator = userAddDtoValidator;
            _addressUpdateDtoValidator = addressUpdateDtoValidator;
        }

        public IWriteRepository<User> Users { get; set; }

        public async Task<bool> AddUserAsync(UserAddDto user)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(user, _userAddDtoValidator);
            await ThrowErrorIfSameMailExists(user.Mail);
            var mappedUser = _mapper.Map<User>(user);
            mappedUser.IsValid = true;
            CreateCartIfRoleIsUser(mappedUser);

            return await _userWriteRepository.AddAsync(mappedUser);
        }

        public async Task<bool> SafeDeleteUserAsync(string userId)
        {
            var user = await GetSingleUser(userId);
            if (!user.IsValid)
                return true;

            return await _userWriteRepository.SafeRemoveAsync(user);
        }

        public async Task<bool> UpdateUser(UserUpdateDto userUpdateDto, string userId)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(userUpdateDto, _userUpdateDtoValidator);
            var user = await GetSingleUser(userId);
            _mapper.Map(userUpdateDto, user);

            return await _userWriteRepository.UpdateAsync(user);
        }

        public async Task<bool> UpdateUserPasswordAsync(string password)
        {
            var user = await GetSingleUser(_httpContextAccessor.HttpContext.User.GetActiveUserId());

            var salt = _passwordGenerationService.GenerateSalt();
            var combinedBytes = _passwordGenerationService.Combine(Encoding.UTF8.GetBytes(password), salt);
            var hashedBytes = _passwordGenerationService.HashBytes(combinedBytes);
            string storedSalt = Convert.ToBase64String(salt);
            string storedHashedPassword = Convert.ToBase64String(hashedBytes);

            user.Password = storedHashedPassword;
            user.PasswordSalt = storedSalt;

            return await _userWriteRepository.UpdateAsync(user);
        }

        public async Task<bool> UpdateAddress(AddressUpdateDto addressUpdateDto)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(addressUpdateDto, _addressUpdateDtoValidator);
            var user = await _userReadService.Users.GetWhere(_ => _.Id == Guid.Parse(_httpContextAccessor.HttpContext.User.GetActiveUserId()), includeProperties: [_ => _.Address]).FirstOrDefaultAsync();
            if (user.Address is null)
                user.Address = _mapper.Map<Address>(addressUpdateDto);
            else
                _mapper.Map(addressUpdateDto, user.Address);

            return await _userWriteRepository.UpdateAsync(user);
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var user = await _userReadService.Users.GetByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("Kullanıcı bulunamadı!");
            if (user.IsValid)
                return true;
            user.IsValid = true;
            user.DeletedDate = DateTime.MinValue;

            return await _userWriteRepository.UpdateAsync(user);
        }


        private async Task<User> GetSingleUser(string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var user = await _userReadService.Users.GetByIdAsync(userId);
            if (user is null || !user.IsValid)
                throw new NotFoundException("Kullanıcı bulunamadı!");

            return user;
        }

        private async Task ThrowErrorIfSameMailExists(string mail)
        {
            var user = await _userReadService.Users.GetSingleAsync(_ => _.Mail == mail);
            if (user != null)
                throw new NotFoundException("Farklı bir mail adresi deneyiniz.");
        }

        private void CreateCartIfRoleIsUser(User user)
        {
            if (user.Role == Role.User)
                user.Cart = new Cart();
        }
    }
}
