using ECommerce.Business.Helpers.TokenServices;
using ECommerce.Business.Services.Addresses;
using ECommerce.Business.Services.Addresses.Abstract;
using ECommerce.Business.Services.Authentication;
using ECommerce.Business.Services.Authentication.Abstract;
using ECommerce.Business.Services.Brands;
using ECommerce.Business.Services.Brands.Abstract;
using ECommerce.Business.Services.Carts;
using ECommerce.Business.Services.Carts.Abstract;
using ECommerce.Business.Services.Categories;
using ECommerce.Business.Services.Categories.Abstract;
using ECommerce.Business.Services.ImageService;
using ECommerce.Business.Services.PaymentCards;
using ECommerce.Business.Services.PaymentCards.Abstract;
using ECommerce.Business.Services.Products;
using ECommerce.Business.Services.Products.Abstract;
using ECommerce.Business.Services.SubCategories;
using ECommerce.Business.Services.SubCategories.Abstract;
using ECommerce.Business.Services.Users;
using ECommerce.Business.Services.Users.Abstract;
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
            services.AddScoped<IAddressReadService, AddressReadService>();
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
