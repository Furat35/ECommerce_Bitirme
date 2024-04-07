using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.SubCategories;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using FluentValidation;

namespace ECommerce.Business.Services.WriteServices
{
    public class SubCategoryWriteService : ISubCategoryWriteService
    {
        private readonly IWriteRepository<SubCategory> _subCategoryWriteRepository;
        private readonly ISubCategoryReadService _subCategoryReadService;
        private readonly IMapper _mapper;
        private readonly IValidator<SubCategoryAddDto> _subCategoryAddDtoValidator;
        private readonly IValidator<SubCategoryUpdateDto> _subCategoryUpdateDtoValidator;

        public SubCategoryWriteService(IUnitOfWork unitOfWork, IMapper mapper, ISubCategoryReadService subCategoryReadService,
            IValidator<SubCategoryAddDto> subCategoryAddDtoValidator, IValidator<SubCategoryUpdateDto> subCategoryUpdateDtoValidator)
        {
            _subCategoryWriteRepository = unitOfWork.GetWriteRepository<SubCategory>();
            _mapper = mapper;
            _subCategoryReadService = subCategoryReadService;
            _subCategoryAddDtoValidator = subCategoryAddDtoValidator;
            _subCategoryUpdateDtoValidator = subCategoryUpdateDtoValidator;
        }

        public async Task<SubCategoryListDto> AddSubCategoryAsync(SubCategoryAddDto subCategory)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(subCategory, _subCategoryAddDtoValidator);
            await ActivateSubCategoryIfDeleted(subCategory.Name, subCategory.CategoryId);
            var subCategoryToAdd = _mapper.Map<SubCategory>(subCategory);
            subCategoryToAdd.IsValid = true;
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
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(subCategory, _subCategoryUpdateDtoValidator);
            await ActivateSubCategoryIfDeleted(subCategory.Name.ToString(), subCategory.CategoryId);
            var categoryToUpdate = await GetSingleSubCategory(subCategory.Id);
            _mapper.Map(subCategory, categoryToUpdate);

            return await _subCategoryWriteRepository.UpdateAsync(categoryToUpdate);
        }

        private async Task<SubCategory> ActivateSubCategoryIfDeleted(string subCategoryName, string categoryId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(categoryId);
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
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(subCategoryId);
            var category = await _subCategoryReadService.SubCategories.GetByIdAsync(subCategoryId);
            return category is null || !category.IsValid
                ? throw new NotFoundException("Alt Kategori bulunamadı!")
                : category;
        }
    }
}
