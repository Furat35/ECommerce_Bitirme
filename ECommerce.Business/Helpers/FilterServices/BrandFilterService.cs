using AutoMapper;
using ECommerce.Business.Helpers.Brands;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class BrandFilterService
    {
        private readonly IMapper _mapper;
        private IQueryable<Brand> _brands;

        public BrandFilterService(IMapper mapper, IQueryable<Brand> brands)
        {
            _mapper = mapper;
            _brands = brands;
        }

        public ProductResponse<List<BrandListDto>> FilterBrands(BrandRequestFilter filters)
        {
            _brands = NameStartsWith(filters.Name);

            Metadata metadata = new(filters.Page, filters.PageSize, _brands.Count(), _brands.Count() / filters.PageSize + 1);
            _brands = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedBrands = _mapper.Map<List<BrandListDto>>(_brands);

            return new()
            {
                ResponseValue = mappedBrands,
                Headers = header
            };
        }

        private IQueryable<Brand> NameStartsWith(string brandName)
         => !string.IsNullOrEmpty(brandName)
            ? _brands.Where(brand => brand.Name.StartsWith(brandName))
            : _brands;

        private IQueryable<Brand> AddPagination(BrandRequestFilter filters)
          => _brands
              .OrderBy(_ => _.Name)
              .Skip(filters.Page * filters.PageSize)
              .Take(filters.PageSize);

    }
}
