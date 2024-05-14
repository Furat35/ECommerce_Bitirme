using AutoMapper;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Business.Services.Contracts.IReadServices;
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
    public class CategoryUnitTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWorkService = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new Mock<IHttpContextAccessor>();
        private readonly Mock<ICategoryReadService> _mockCategoryReadService = new Mock<ICategoryReadService>();
        private readonly Mock<IValidator<CategoryAddDto>> _mockCategoryAddDtoValidator = new Mock<IValidator<CategoryAddDto>>();
        private readonly Mock<IValidator<CategoryUpdateDto>> _mockCategoryUpdateDtoValidator = new Mock<IValidator<CategoryUpdateDto>>();
        private EfECommerceContext dbContext;


        public CategoryUnitTests()
        {
            CreateMemoryDbContext();
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Category>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Category, object>>[]>()))
              .ReturnsAsync(dbContext.Categories.First());
        }

        public EfECommerceContext CreateMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<EfECommerceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;

            dbContext = new EfECommerceContext(options);
            dbContext.Categories.AddRange(new Category { Id = Guid.Parse("8963B9A6-30D4-481B-B421-214FCC2640FF"), Name = "Elektronik", IsValid = true, CreatedDate = DateTime.Now });
            dbContext.SaveChanges();

            return dbContext;
        }

        // Get category by id
        [Fact]
        public void GetCategoryById_WithValidCategoryId_ReturnsCategoryWithGivenId()
        {
            // Arrange
            var category = dbContext.Categories.First();
            _mockMapper.Setup(_ => _.Map<CategoryListDto>(It.IsAny<object>()))
                .Returns(new CategoryListDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            var categoryReadService = new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object);

            // Act
            var categoryListDto = categoryReadService.GetCategoryByIdAsync(category.Id.ToString());

            // Assert
            Assert.NotNull(categoryListDto);
        }

        // Get category by invalid id
        [Fact]
        public async Task GetCategoryById_WithInvalidValidCategoryId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidCategoryId = "8963B9A6-30D4-481B-B421-214FCC26F4";
            _mockMapper.Setup(_ => _.Map<CategoryListDto>(It.IsAny<object>()))
                .Returns((CategoryListDto)null);
            var categoryReadService = new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object);

            // Act
            var categoryListDto = async () => await categoryReadService.GetCategoryByIdAsync(invalidCategoryId);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), categoryListDto);
        }

        // Add category 
        [Fact]
        public async Task AddCategory_WithValidCategoryProperties_ReturnsCreatedCategory()
        {
            // Arrange
            _mockCategoryAddDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CategoryAddDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(_ => _.Map<Category>(It.IsAny<object>()))
              .Returns(new Category());
            _mockMapper.Setup(_ => _.Map<CategoryListDto>(It.IsAny<object>()))
                .Returns(new CategoryListDto());
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().UpdateAsync(It.IsAny<Category>()));
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Category>().GetSingleAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Category, object>>[]>()))
                .ReturnsAsync((Category)null);

            var categoryReadService = new CategoryWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockCategoryAddDtoValidator.Object, _mockCategoryUpdateDtoValidator.Object);

            // Act
            var addedCategory = await categoryReadService.AddCategoryAsync(new CategoryAddDto());

            // Assert
            Assert.NotNull(addedCategory);
        }

        // Add category with invalid properties
        [Fact]
        public async Task AddCategory_WithInvalidCategoryProperties_ThrowsBadRequestException()
        {
            // Arrange
            _mockCategoryAddDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CategoryAddDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorCode = "BadRequest", ErrorMessage = "BAD REQUEST" } } });

            _mockMapper.Setup(_ => _.Map<Category>(It.IsAny<object>()))
              .Returns(new Category());
            _mockMapper.Setup(_ => _.Map<CategoryListDto>(It.IsAny<object>()))
                .Returns(new CategoryListDto());
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().UpdateAsync(It.IsAny<Category>()));
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Category>().GetSingleAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Category, object>>[]>()))
                .ReturnsAsync((Category)null);

            var categoryReadService = new CategoryWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockCategoryAddDtoValidator.Object, _mockCategoryUpdateDtoValidator.Object);

            // Act
            var badRequestException = async () => await categoryReadService.AddCategoryAsync(new CategoryAddDto());

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), badRequestException);
        }

        // Update category with invalid properties
        [Fact]
        public async Task UpdateCategory_WithInvalidCategoryProperties_ReturnsTrue()
        {
            // Arrange
            _mockCategoryUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CategoryUpdateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorCode = "BadRequest", ErrorMessage = "BAD REQUEST" } } });

            _mockMapper.Setup(_ => _.Map(It.IsAny<object>(), It.IsAny<object>()))
              .Returns(new Category());

            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Category>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Category, object>>[]>()))
                .ReturnsAsync(new Category { IsValid = true });

            var categoryReadService = new CategoryWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockCategoryAddDtoValidator.Object, _mockCategoryUpdateDtoValidator.Object);

            // Act
            var response = async () => await categoryReadService.UpdateCategoryAsync(new CategoryUpdateDto { Id = "8963B9A6-30D4-481B-B421-214FCC2640FF" });

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), response);
        }

        // Remove category
        [Fact]
        public async Task RemoveCategory_WithValidCategoryId_ReturnsTrue()
        {
            // Arrange
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().SafeRemoveAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Category>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Category, object>>[]>()))
                .ReturnsAsync(new Category { Id = Guid.Parse("8963B9A6-30D4-481B-B421-214FCC2640FF"), IsValid = true });

            var categoryReadService = new CategoryWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockCategoryAddDtoValidator.Object, _mockCategoryUpdateDtoValidator.Object);

            // Act
            var response = await categoryReadService.SafeRemoveCategoryAsync("8963B9A6-30D4-481B-B421-214FCC2640FF");

            // Assert
            Assert.True(response);
        }

        // Remove category with invalid id
        [Fact]
        public async Task RemoveCategory_WithInvalidCategoryId_ReturnsTrue()
        {
            // Arrange
            string invalidGuid = "8963B96-30D4-481B-B421-214FCC2640";
            _mockUnitOfWorkService.Setup(_ => _.GetWriteRepository<Category>().SafeRemoveAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);
            _mockUnitOfWorkService.Setup(_ => _.GetReadRepository<Category>().GetByIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Category, object>>[]>()))
                .ReturnsAsync(new Category());

            var categoryReadService = new CategoryWriteService(_mockUnitOfWorkService.Object, _mockMapper.Object, new CategoryReadService(_mockUnitOfWorkService.Object, _mockMapper.Object, _mockHttpContext.Object),
                _mockCategoryAddDtoValidator.Object, _mockCategoryUpdateDtoValidator.Object);

            // Act
            var response = async () => await categoryReadService.SafeRemoveCategoryAsync(invalidGuid);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), response);
        }
    }
}
