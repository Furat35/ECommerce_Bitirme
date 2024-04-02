using AutoMapper;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.WriteServices
{
    public class SubCategoryWriteService : ISubCategoryWriteService
    {
        private readonly IWriteRepository<SubCategory> _subCategoryWriteRepository;
        private readonly ISubCategoryReadService _subCategoryReadService;
        private readonly IMapper _mapper;

        public SubCategoryWriteService(IUnitOfWork unitOfWork, IMapper mapper, ISubCategoryReadService subCategoryReadService)
        {
            _subCategoryWriteRepository = unitOfWork.GetWriteRepository<SubCategory>();
            _mapper = mapper;
            _subCategoryReadService = subCategoryReadService;
        }

        public async Task<SubCategoryListDto> AddSubCategoryAsync(SubCategoryAddDto subCategory)
        {
            await ActivateSubCategoryIfDeleted(subCategory.Name, subCategory.CategoryId);
            var subCategoryToAdd = _mapper.Map<SubCategory>(subCategory);
            bool isAdded = await _subCategoryWriteRepository.AddAsync(subCategoryToAdd);
            if (!isAdded)
                throw new InternalServerErrorException();

            return _mapper.Map<SubCategoryListDto>(subCategoryToAdd);
        }

        public async Task<bool> SafeRemoveSubCategoryAsync(string subCategoryId)
        {
            var subCategory = await GetSingleSubCategory(subCategoryId);
            return await _subCategoryWriteRepository.SafeRemoveAsync(subCategory);
        }

        public async Task<bool> UpdateSubCategoryAsync(SubCategoryUpdateDto subCategory)
        {
            await ActivateSubCategoryIfDeleted(subCategory.Name.ToString(), subCategory.CategoryId);
            var categoryToUpdate = await GetSingleSubCategory(subCategory.Id);
            _mapper.Map(subCategory, categoryToUpdate);

            return await _subCategoryWriteRepository.UpdateAsync(categoryToUpdate);
        }

        private async Task<SubCategory> ActivateSubCategoryIfDeleted(string subCategoryName, string categoryId)
        {
            var subCategory = await _subCategoryReadService.SubCategories.GetSingleAsync(_ => _.Name == subCategoryName && _.CategoryId.ToString() == categoryId);
            if (subCategory != null)
            {
                if (!subCategory.IsValid)
                {
                    subCategory.DeletedDate = DateTime.MinValue;
                    subCategory.IsValid = true;
                    await _subCategoryWriteRepository.UpdateAsync(subCategory);
                }

                throw new BadRequestException("Alt Kategori bulunmaktadır!");
            }

            return subCategory;
        }

        private async Task<SubCategory> GetSingleSubCategory(string subCategoryId)
        {
            var category = await _subCategoryReadService.SubCategories.GetByIdAsync(subCategoryId);
            return category is null || !category.IsValid
                ? throw new NotFoundException("Alt Kategori bulunamadı!")
                : category;
        }
    }
}
