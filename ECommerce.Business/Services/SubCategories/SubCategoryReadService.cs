using AutoMapper;
using ECommerce.Business.Helpers.FilterServices;
using ECommerce.Business.Helpers.HeaderServices;
using ECommerce.Business.Helpers.SubCategories;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Business.Services.SubCategories.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ECommerce.Business.Services.SubCategories
{
    public class SubCategoryReadService : ISubCategoryReadService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public SubCategoryReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            SubCategories = unitOfWork.GetReadRepository<SubCategory>();
            _mapper = mapper;
            _httpContext = contextAccessor;
        }

        public IReadRepository<SubCategory> SubCategories { get; }

        public List<SubCategoryListDto> GetSubCategoriesWhere(SubCategoryRequestFilter filters, Expression<Func<SubCategory, bool>> predicate)
        {
            var subCategories = SubCategories.GetWhere(predicate, includeProperties: [_ => _.Category]);
            var filteredCategorys = new SubCategoryFilterService(_mapper, subCategories).FilterCategories(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredCategorys.Headers);

            return filteredCategorys.ResponseValue;
        }

        public async Task<SubCategoryListDto> GetSubCategoryByIdAsync(string subCategoryId)
        {
            var category = await GetSingleSubCategory(subCategoryId);
            return _mapper.Map<SubCategoryListDto>(category);
        }

        private async Task<SubCategory> GetSingleSubCategory(string subCategoryId)
        {
            var subCategory = await SubCategories.GetByIdAsync(subCategoryId, includeProperties: [_ => _.Category]);
            if (subCategory is null || !subCategory.IsValid)
                throw new NotFoundException("Alt Kategori bulunamadı!");

            return subCategory;
        }
    }
}
