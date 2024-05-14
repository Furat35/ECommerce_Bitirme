using AutoMapper;
using ECommerce.Business.Models.Dtos.Products;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Business.Services.ImageService;
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
using System.Security.Claims;

namespace ECommerce.Tests.UnitTests
{
    public class ProductUnitTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWorkService = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new Mock<IHttpContextAccessor>();
        private readonly Mock<IFileService> _mockFileServiceContext = new Mock<IFileService>();
        private readonly Mock<IProductReadService> _mockProductReadService = new Mock<IProductReadService>();
        private readonly Mock<IProductPhotoWriteService> _mockProductPhotoWriteService = new Mock<IProductPhotoWriteService>();
        private readonly Mock<ISubCategoryReadService> _mockSubCategoryReadService = new Mock<ISubCategoryReadService>();
        private readonly Mock<IBrandReadService> _mockBrandReadService = new Mock<IBrandReadService>();
        private readonly Mock<IValidator<ProductAddDto>> _mockProductAddDtoValidator = new Mock<IValidator<ProductAddDto>>();
        private readonly Mock<IValidator<ProductUpdateDto>> _mockProductUpdateDtoValidator = new Mock<IValidator<ProductUpdateDto>>();
        private EfECommerceContext dbContext;


        public ProductUnitTests()
        {
            CreateMemoryDbContext();
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Product>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>[]>()))
              .ReturnsAsync(dbContext.Products.First());

            // Create a ClaimsPrincipal with the desired claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "48DF4AF4-AA79-45F2-755C-08DC51134E88"), // Replace "1234567890" with your desired user identifier
                // Add other claims as needed
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _mockHttpContext.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

        }

        public EfECommerceContext CreateMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<EfECommerceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;

            dbContext = new EfECommerceContext(options);
            dbContext.Products.AddRange(
                new Product
                {
                    ProductName = "İPhone 15",
                    SubProductName = "256 gb siyah",
                    CreatedBy = Guid.Parse("48DF4AF4-AA79-45F2-755C-08DC51134E88"),
                    ProductDescription = "Özellikleri vs.",
                    Stock = 50,
                    Price = 222,
                    SubCategory = new SubCategory
                    {
                        Name = "Telefon",
                        IsValid = true,
                        CreatedDate = DateTime.Now,
                        Category = new Category { Name = "Elektronik", IsValid = true, CreatedDate = DateTime.Now }
                    },
                    Brand = new Brand { Name = "Apple", CreatedDate = DateTime.Now, IsValid = true },
                    IsValid = true,
                    CreatedDate = DateTime.Now
                });
            dbContext.SaveChanges();

            return dbContext;
        }

        // Get product by id
        [Fact]
        public void GetProductById_WithValidProductId_ReturnsProductWithGivenId()
        {
            // Arrange
            var product = dbContext.Products.First();
            _mockMapper.Setup(_ => _.Map<ProductListDto>(It.IsAny<object>()))
                .Returns(new ProductListDto
                {
                    Id = product.Id,
                });
            var productReadService = new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object);

            // Act
            var productListDto = productReadService.GetProductByIdAsync(product.Id.ToString());

            // Assert
            Assert.NotNull(productListDto);
        }

        // Get product by invalid id
        [Fact]
        public async Task GetProductById_WithInvalidValidProductId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidProductId = "8963B9A6-30D4-481B-B421-214FCC26F4";
            _mockMapper.Setup(_ => _.Map<ProductListDto>(It.IsAny<object>()))
                .Returns((ProductListDto)null);
            var productReadService = new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object);

            // Act
            var productListDto = async () => await productReadService.GetProductByIdAsync(invalidProductId);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), productListDto);
        }

        // Add product 
        [Fact]
        public async Task AddProduct_WithValidProductProperties_ReturnsCreatedProduct()
        {
            // Arrange
            _mockProductAddDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<ProductAddDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(_ => _.Map<Product>(It.IsAny<object>()))
              .Returns(new Product());
            _mockMapper.Setup(_ => _.Map<ProductListDto>(It.IsAny<object>()))
                .Returns(new ProductListDto());
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Product>().AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            var productReadService = new ProductWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object),
                _mockProductPhotoWriteService.Object, _mockSubCategoryReadService.Object, _mockBrandReadService.Object, _mockHttpContext.Object, _mockProductAddDtoValidator.Object, _mockProductUpdateDtoValidator.Object);

            // Act
            var addedProduct = await productReadService.AddProductAsync(new ProductAddDto());

            // Assert
            Assert.NotNull(addedProduct);
        }

        // Add product with invalid properties
        [Fact]
        public async Task AddProduct_WithInvalidProductProperties_ThrowsBadRequestException()
        {
            // Arrange
            _mockProductAddDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<ProductAddDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorCode = "BadRequest", ErrorMessage = "BAD REQUEST" } } });

            _mockMapper.Setup(_ => _.Map<Product>(It.IsAny<object>()))
              .Returns(new Product());
            _mockMapper.Setup(_ => _.Map<ProductListDto>(It.IsAny<object>()))
                .Returns(new ProductListDto());
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Product>().AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            var productReadService = new ProductWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object),
                _mockProductPhotoWriteService.Object, _mockSubCategoryReadService.Object, _mockBrandReadService.Object, _mockHttpContext.Object, _mockProductAddDtoValidator.Object, _mockProductUpdateDtoValidator.Object);

            // Act
            var badRequestException = async () => await productReadService.AddProductAsync(new ProductAddDto());

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), badRequestException);
        }

        // Update product with invalid properties
        [Fact]
        public async Task UpdateProduct_WithInvalidProductProperties_ReturnsTrue()
        {
            // Arrange
            _mockProductUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<ProductUpdateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorCode = "BadRequest", ErrorMessage = "BAD REQUEST" } } });

            _mockMapper.Setup(_ => _.Map(It.IsAny<object>(), It.IsAny<object>()))
              .Returns(new Product());

            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Product>().UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Product>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                .ReturnsAsync(new Product { IsValid = true });

            var productReadService = new ProductWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object),
                   _mockProductPhotoWriteService.Object, _mockSubCategoryReadService.Object, _mockBrandReadService.Object, _mockHttpContext.Object, _mockProductAddDtoValidator.Object, _mockProductUpdateDtoValidator.Object);

            // Act
            var response = async () => await productReadService.UpdateProductAsync(new ProductUpdateDto());

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), response);
        }

        // Remove product
        [Fact]
        public async Task RemoveProduct_WithValidProductId_ReturnsTrue()
        {
            // Arrange
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Product>().SafeRemoveAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Product>().GetSingleAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                .ReturnsAsync(new Product { Id = Guid.Parse("8963B9A6-30D4-481B-B421-214FCC2640FF"), IsValid = true });

            var productReadService = new ProductWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object),
              _mockProductPhotoWriteService.Object, _mockSubCategoryReadService.Object, _mockBrandReadService.Object, _mockHttpContext.Object, _mockProductAddDtoValidator.Object, _mockProductUpdateDtoValidator.Object);

            // Act
            var response = await productReadService.SafeRemoveProductAsync("8963B9A6-30D4-481B-B421-214FCC2640FF");

            // Assert
            Assert.True(response);
        }

        // Remove product with invalid id
        [Fact]
        public async Task RemoveProduct_WithInvalidProductId_ReturnsTrue()
        {
            // Arrange
            string invalidGuid = "8963B96-30D4-481B-B421-214FCC2640";
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Product>().SafeRemoveAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Product>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                .ReturnsAsync(new Product());

            var productReadService = new ProductWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new ProductReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object, _mockFileServiceContext.Object),
                   _mockProductPhotoWriteService.Object, _mockSubCategoryReadService.Object, _mockBrandReadService.Object, _mockHttpContext.Object, _mockProductAddDtoValidator.Object, _mockProductUpdateDtoValidator.Object);

            // Act
            var response = async () => await productReadService.SafeRemoveProductAsync(invalidGuid);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), response);
        }
    }
}
