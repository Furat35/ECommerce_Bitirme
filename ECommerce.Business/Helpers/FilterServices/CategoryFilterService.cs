using AutoMapper;
using ECommerce.Business.Helpers.Categories;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class CategoryFilterService
    {
        private readonly IMapper _mapper;
        private IQueryable<Category> _categories;

        public CategoryFilterService(IMapper mapper, IQueryable<Category> categories)
        {
            _mapper = mapper;
            _categories = categories;
        }

        public ProductResponse<List<CategoryListDto>> FilterCategories(CategoryRequestFilter filters)
        {
            _categories = NameStartsWith(filters.Name);
            int pageNumber;
            pageNumber = _categories.Count() % filters.PageSize == 0 ? _categories.Count() / filters.PageSize : _categories.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _categories.Count(), pageNumber);
            _categories = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedCategorys = _mapper.Map<List<CategoryListDto>>(_categories);

            return new()
            {
                ResponseValue = mappedCategorys,
                Headers = header
            };
        }

        private IQueryable<Category> NameStartsWith(string categoryName)
         => !string.IsNullOrEmpty(categoryName)
            ? _categories.Where(category => category.Name.StartsWith(categoryName))
            : _categories;

        private IQueryable<Category> AddPagination(CategoryRequestFilter filters)
          => _categories
              .OrderBy(_ => _.Name)
              .Skip((filters.Page - 1) * filters.PageSize)
              .Take(filters.PageSize);
    }
}
