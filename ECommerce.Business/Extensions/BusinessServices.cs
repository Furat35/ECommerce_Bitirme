using ECommerce.Business.Helpers.TokenServices;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Business.Services.ImageService;
using ECommerce.Business.Services.ReadServices;
using ECommerce.Business.Services.WriteServices;
using ECommerce.Business.Validations.FluentValidations.Products;
using ECommerce.Core.Helpers.PasswordServices;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Business.Extensions
{
    public static class BusinessServices
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DI servisleri
            services.AddScoped<IProductReadService, ProductReadService>();
            services.AddScoped<IProductWriteService, ProductWriteService>();
            services.AddScoped<IUserReadService, UserReadService>();
            services.AddScoped<IUserWriteService, UserWriteService>();
            services.AddScoped<IBrandReadService, BrandReadService>();
            services.AddScoped<IBrandWriteService, BrandWriteService>();
            services.AddScoped<ICategoryReadService, CategoryReadService>();
            services.AddScoped<ICategoryWriteService, CategoryWriteService>();
            services.AddScoped<IPaymentCardReadService, PaymentCardReadService>();
            services.AddScoped<IPaymentCardWriteService, PaymentCardWriteService>();
            services.AddScoped<ISubCategoryReadService, SubCategoryReadService>();
            services.AddScoped<ISubCategoryWriteService, SubCategoryWriteService>();
            services.AddScoped<ICartReadService, CartReadService>();
            services.AddScoped<ICartWriteService, CartWriteService>();
            services.AddScoped<IUserWriteService, UserWriteService>();
            services.AddScoped<IOrderWriteService, OrderWriteService>();
            services.AddScoped<IAddressReadService, AddressReadService>();
            services.AddScoped<IProductPhotoWriteService, ProductPhotoWriteService>();
            services.AddScoped<IAuthWriteService, AuthWriteService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPasswordGenerationService, PasswordGenerationService>();
            services.AddScoped<ITokenService, JwtService>();

            // Automapper implementasyonu 
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Fluentvalidation implementasyonu
            services.AddValidatorsFromAssemblyContaining<ProductAddDtoValidator>();

            return services;
        }

    }
}
