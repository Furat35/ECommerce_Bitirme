using AutoMapper;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Helpers.SubCategories;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class SubCategoryFilterService
    {
        private readonly IMapper _mapper;
        private IQueryable<SubCategory> _subCategories;

        public SubCategoryFilterService(IMapper mapper, IQueryable<SubCategory> subCategories)
        {
            _mapper = mapper;
            _subCategories = subCategories;
        }

        public ProductResponse<List<SubCategoryListDto>> FilterCategories(SubCategoryRequestFilter filters)
        {
            int pageNumber;
            pageNumber = _subCategories.Count() % filters.PageSize == 0 ? _subCategories.Count() / filters.PageSize : _subCategories.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _subCategories.Count(), pageNumber);
            _subCategories = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedCategorys = _mapper.Map<List<SubCategoryListDto>>(_subCategories);

            return new()
            {
                ResponseValue = mappedCategorys,
                Headers = header
            };
        }

        private IQueryable<SubCategory> AddPagination(SubCategoryRequestFilter filters)
          => _subCategories
              .OrderBy(_ => _.Name)
              .Skip((filters.Page - 1) * filters.PageSize)
              .Take(filters.PageSize);
    }
}
