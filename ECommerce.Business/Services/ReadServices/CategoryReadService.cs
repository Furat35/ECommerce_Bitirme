using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Helpers.Categories;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.ReadServices
{
    public class CategoryReadService : ICategoryReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public CategoryReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            Categories = unitOfWork.GetReadRepository<Category>();
            _mapper = mapper;
            _httpContext = contextAccessor;
        }

        public IReadRepository<Category> Categories { get; }

        public async Task<CategoryListDto> GetCategoryByIdAsync(string categoryId)
        {
            var category = await GetSingleCategory(categoryId);
            return _mapper.Map<CategoryListDto>(category);
        }

        public List<CategoryListDto> GetCategoriesWhere(CategoryRequestFilter filters, Expression<Func<Category, bool>> predicate)
        {
            var categories = Categories.GetWhere(predicate, includeProperties: [_ => _.SubCategories]).Select(_ => new Category
            {
                Id = _.Id,
                Name = _.Name,
                SubCategories = _.SubCategories.Where(_ => _.IsValid).ToList()
            });

            categories.Where(_ => _.SubCategories.Where(_ => _.IsValid) != null);
            var filteredCategorys = new CategoryFilterService(_mapper, categories).FilterCategories(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredCategorys.Headers);

            return filteredCategorys.ResponseValue;
        }

        private async Task<Category> GetSingleCategory(string categoryId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(categoryId);
            var category = await Categories.GetByIdAsync(categoryId, includeProperties: [_ => _.SubCategories]);
            if (category is null || !category.IsValid)
                throw new NotFoundException("Kategori bulunamadı!");

            return category;
        }
    }
}
