using AutoMapper;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Business.Services.ReadServices;
using ECommerce.Business.Services.WriteServices;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.Repositories.Contexts;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace ECommerce.Tests.UnitTests
{
    public class BrandUnitTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWorkService = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new Mock<IHttpContextAccessor>();
        private readonly Mock<IValidator<BrandAddDto>> _mockBrandAddDtoValidator = new Mock<IValidator<BrandAddDto>>();
        private readonly Mock<IValidator<BrandUpdateDto>> _mockBrandUpdateDtoValidator = new Mock<IValidator<BrandUpdateDto>>();
        private EfECommerceContext dbContext;

        public BrandUnitTests()
        {
            CreateMemoryDbContext();
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
              .ReturnsAsync(dbContext.Brands.First());
        }

        public EfECommerceContext CreateMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<EfECommerceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;

            dbContext = new EfECommerceContext(options);
            dbContext.Brands.AddRange(
                new Brand()
                {
                    Id = Guid.Parse("8963B9A6-30D4-481B-B421-214FCC2640FF"),
                    Name = "Electronic",
                    Products = new List<Product>()
                    {
                         new Product{ ProductName = "İPhone 15", SubProductName = "256 gb siyah", CreatedBy = Guid.Parse("48DF4AF4-AA79-45F2-755C-08DC51134E88"),
                            ProductDescription = "Özellikleri vs.", Stock = 50, Price = 222,
                            SubCategory = new SubCategory{Name = "Telefon", IsValid = true, CreatedDate = DateTime.Now,
                            Category = new Category{ Name = "Electronic", IsValid = true, CreatedDate = DateTime.Now}},
                            Brand = new Brand{ Name = "Apple",CreatedDate = DateTime.Now, IsValid = true},
                            IsValid = true, CreatedDate = DateTime.Now  }
                    }
                });
            dbContext.SaveChanges();

            return dbContext;
        }

        // Get brand by id
        [Fact]
        public void GetBrandById_WithValidBrandId_ReturnsBrandWithGivenId()
        {
            // Arrange
            var brand = dbContext.Brands.First();
            _mockMapper.Setup(_ => _.Map<BrandListDto>(It.IsAny<object>()))
                .Returns(new BrandListDto
                {
                    Id = brand.Id,
                    Name = brand.Name
                });
            var brandReadService = new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object);

            // Act
            var brandListDto = brandReadService.GetBrandByIdAsync(brand.Id.ToString());

            // Assert
            Assert.NotNull(brandListDto);
        }

        // Get brand by invalid id
        [Fact]
        public async Task GetBrandById_WithInvalidValidBrandId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidBrandId = Guid.Parse("8963B9A6-30D4-481B-B421-214FCC2640F4");
            _mockMapper.Setup(_ => _.Map<BrandListDto>(It.IsAny<object>()))
                .Returns((BrandListDto)null);
            var brandReadService = new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object);

            // Act
            var brandListDto = async () => await brandReadService.GetBrandByIdAsync(invalidBrandId.ToString());

            // Assert
            await Assert.ThrowsAsync(typeof(NotFoundException), brandListDto);
        }

        // Add brand 
        [Fact]
        public async Task AddBrand_WithValidBrandProperties_ReturnsCreatedBrand()
        {
            // Arrange
            _mockBrandAddDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<BrandAddDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(_ => _.Map<Brand>(It.IsAny<object>()))
              .Returns(new Brand());
            _mockMapper.Setup(_ => _.Map<BrandListDto>(It.IsAny<object>()))
                .Returns(new BrandListDto());
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().AddAsync(It.IsAny<Brand>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().UpdateAsync(It.IsAny<Brand>()));
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetSingleAsync(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
                .ReturnsAsync((Brand)null);

            var brandReadService = new BrandWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockBrandAddDtoValidator.Object, _mockBrandUpdateDtoValidator.Object);

            // Act
            var addedBrand = await brandReadService.AddBrandAsync(new BrandAddDto());

            // Assert
            Assert.NotNull(addedBrand);
        }

        // Add brand with invalid properties
        [Fact]
        public async Task AddBrand_WithInvalidBrandProperties_ThrowsBadRequestException()
        {
            // Arrange
            _mockBrandAddDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<BrandAddDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorCode = "BadRequest", ErrorMessage = "BAD REQUEST" } } });

            _mockMapper.Setup(_ => _.Map<Brand>(It.IsAny<object>()))
              .Returns(new Brand());
            _mockMapper.Setup(_ => _.Map<BrandListDto>(It.IsAny<object>()))
                .Returns(new BrandListDto());
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().AddAsync(It.IsAny<Brand>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().UpdateAsync(It.IsAny<Brand>()));
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetSingleAsync(It.IsAny<Expression<Func<Brand, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
                .ReturnsAsync(dbContext.Brands.First());

            var brandReadService = new BrandWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockBrandAddDtoValidator.Object, _mockBrandUpdateDtoValidator.Object);

            // Act
            var badRequestException = async () => await brandReadService.AddBrandAsync(new BrandAddDto());

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), badRequestException);
        }

        // Update brand
        [Fact]
        public async Task UpdateBrand_WithValidBrandProperties_ReturnsTrue()
        {
            // Arrange
            _mockBrandUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<BrandUpdateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(_ => _.Map(It.IsAny<object>(), It.IsAny<object>()))
              .Returns(new Brand());

            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().UpdateAsync(It.IsAny<Brand>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
                .ReturnsAsync(new Brand { IsValid = true });

            var brandReadService = new BrandWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockBrandAddDtoValidator.Object, _mockBrandUpdateDtoValidator.Object);

            // Act
            var updateResult = await brandReadService.UpdateBrandAsync(new BrandUpdateDto { Id = "8963B9A6-30D4-481B-B421-214FCC2640FF" });

            // Assert
            Assert.True(updateResult);
        }

        // Update brand with invalid properties
        [Fact]
        public async Task UpdateBrand_WithInvalidBrandProperties_ReturnsTrue()
        {
            // Arrange
            _mockBrandUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<BrandUpdateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorCode = "BadRequest", ErrorMessage = "BAD REQUEST" } } });

            _mockMapper.Setup(_ => _.Map(It.IsAny<object>(), It.IsAny<object>()))
              .Returns(new Brand());

            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().UpdateAsync(It.IsAny<Brand>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
                .ReturnsAsync(new Brand { IsValid = true });

            var brandReadService = new BrandWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockBrandAddDtoValidator.Object, _mockBrandUpdateDtoValidator.Object);

            // Act
            var response = async () => await brandReadService.UpdateBrandAsync(new BrandUpdateDto { Id = "8963B9A6-30D4-481B-B421-214FCC2640FF" });

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), response);
        }

        // Remove brand
        [Fact]
        public async Task RemoveBrand_WithValidBrandId_ReturnsTrue()
        {
            // Arrange
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().SafeRemoveAsync(It.IsAny<Brand>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
                .ReturnsAsync(new Brand { Id = Guid.Parse("8963B9A6-30D4-481B-B421-214FCC2640FF"), IsValid = true });

            var brandReadService = new BrandWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockBrandAddDtoValidator.Object, _mockBrandUpdateDtoValidator.Object);

            // Act
            var response = await brandReadService.SafeRemoveBrandAsync("8963B9A6-30D4-481B-B421-214FCC2640FF");

            // Assert
            Assert.True(response);
        }

        // Remove brand with invalid id
        [Fact]
        public async Task RemoveBrand_WithInvalidBrandId_ReturnsTrue()
        {
            // Arrange
            string invalidGuid = "8963B96-30D4-481B-B421-214FCC2640";
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Brand>().SafeRemoveAsync(It.IsAny<Brand>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Brand>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Brand, object>>[]>()))
                .ReturnsAsync(new Brand());

            var brandReadService = new BrandWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new BrandReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockBrandAddDtoValidator.Object, _mockBrandUpdateDtoValidator.Object);

            // Act
            var response = async () => await brandReadService.SafeRemoveBrandAsync(invalidGuid);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), response);
        }

    }
}
