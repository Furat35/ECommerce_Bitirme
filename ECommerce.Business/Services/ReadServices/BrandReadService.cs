using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Helpers.Brands;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.ReadServices
{
    public class BrandReadService : IBrandReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public BrandReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            Brands = unitOfWork.GetReadRepository<Brand>();
            _mapper = mapper;
            _httpContext = contextAccessor;
        }

        public IReadRepository<Brand> Brands { get; }

        public async Task<BrandListDto> GetBrandByIdAsync(string brandId)
        {
            var brand = await GetSingleBrand(brandId);
            return brand != null ? _mapper.Map<BrandListDto>(brand) : null;
        }

        public List<BrandListDto> GetBrandsWhere(BrandRequestFilter filters, Expression<Func<Brand, bool>> predicate)
        {
            var brands = Brands.GetWhere(predicate);
            var filteredBrands = new BrandFilterService(_mapper, brands).FilterBrands(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredBrands.Headers);

            return filteredBrands.ResponseValue;
        }

        private async Task<Brand> GetSingleBrand(string brandId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(brandId);
            var brand = await Brands.GetByIdAsync(brandId);
            if (brand is null || !brand.IsValid)
                throw new NotFoundException("Marka bulunamadı!");

            return brand;
        }
    }
}
