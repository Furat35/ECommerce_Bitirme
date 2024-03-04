using AutoMapper;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Business.Services.Users.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using ECommerce.Core.Helpers.PasswordServices;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using ECommerce.Entity.Enums;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ECommerce.Business.Services.Users
{
    public class UserWriteService : IUserWriteService
    {
        private readonly IMapper _mapper;
        private readonly IWriteRepository<User> _userWriteRepository;
        private readonly IUserReadService _userReadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordGenerationService _passwordGenerationService;

        public UserWriteService(IMapper mapper, IUnitOfWork unitOfWork, IUserReadService userReadService,
            IHttpContextAccessor httpContextAccessor, IPasswordGenerationService passwordGenerationService)
        {
            _userWriteRepository = unitOfWork.GetWriteRepository<User>();
            _userReadService = userReadService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _passwordGenerationService = passwordGenerationService;
        }

        public async Task<bool> AddUserAsync(UserAddDto user)
        {
            await ThrowErrorIfSameMailExists(user.Mail);
            var mappedUser = _mapper.Map<User>(user);
            CreateCartIfRoleIsUser(mappedUser);
            await _userWriteRepository.AddAsync(mappedUser);

            return await SaveChangesAsync();
        }

        public async Task<bool> SafeDeleteUserAsync(string userId)
        {
            var user = await GetSingleUser(userId);
            if (!user.IsValid)
                return true;
            user.DeletedDate = DateTime.Now;
            _userWriteRepository.SafeRemove(user);

            return await SaveChangesAsync();
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
            _userWriteRepository.Update(user);

            return await SaveChangesAsync();
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            var user = await _userReadService.Users.GetByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("Kullanıcı bulunamadı!");
            if (user.IsValid)
                return true;
            user.IsValid = true;
            user.DeletedDate = DateTime.MinValue;
            _userWriteRepository.Update(user);

            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync()
            => await _userWriteRepository.SaveAsync() != 0;

        private async Task<User> GetSingleUser(string userId)
        {
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
