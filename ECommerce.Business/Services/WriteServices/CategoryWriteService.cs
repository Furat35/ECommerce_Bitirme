﻿using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.Categories;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using FluentValidation;

namespace ECommerce.Business.Services.WriteServices
{
    public class CategoryWriteService : ICategoryWriteService
    {
        private readonly IWriteRepository<Category> _categoryWriteRepository;
        private readonly ICategoryReadService _categoryReadService;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryAddDto> _categoryAddDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateDtoValidator;
        public CategoryWriteService(IUnitOfWork unitOfWork, IMapper mapper, ICategoryReadService categoryReadService,
            IValidator<CategoryAddDto> categoryAddDtoValidator, IValidator<CategoryUpdateDto> categoryUpdateDtoValidator)
        {
            _categoryWriteRepository = unitOfWork.GetWriteRepository<Category>();
            _mapper = mapper;
            _categoryReadService = categoryReadService;
            _categoryAddDtoValidator = categoryAddDtoValidator;
            _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
        }

        public async Task<CategoryListDto> AddCategoryAsync(CategoryAddDto category)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(category, _categoryAddDtoValidator);
            await ActivateCategoryIfDeleted(category.Name);
            var categoryToAdd = _mapper.Map<Category>(category);
            categoryToAdd.IsValid = true;
            bool isAdded = await _categoryWriteRepository.AddAsync(categoryToAdd);

            return isAdded ? _mapper.Map<CategoryListDto>(categoryToAdd) : null;
        }

        public async Task<bool> SafeRemoveCategoryAsync(string categoryId)
        {
            var category = await GetSingleCategory(categoryId);
            return await _categoryWriteRepository.SafeRemoveAsync(category);
        }

        public async Task<bool> UpdateCategoryAsync(CategoryUpdateDto category)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(category, _categoryUpdateDtoValidator);
            await ActivateCategoryIfDeleted(category.Name.ToString());
            var categoryToUpdate = await GetSingleCategory(category.Id);
            _mapper.Map(category, categoryToUpdate);

            return await _categoryWriteRepository.UpdateAsync(categoryToUpdate);
        }

        private async Task<Category> ActivateCategoryIfDeleted(string categoryName)
        {
            var category = await _categoryReadService.Categories.GetSingleAsync(_ => _.Name == categoryName);
            if (category != null)
            {
                if (!category.IsValid)
                {
                    category.DeletedDate = DateTime.MinValue;
                    category.IsValid = true;
                    await _categoryWriteRepository.UpdateAsync(category);

                    return category;
                }

                throw new BadRequestException("Kategori bulunmaktadır!");
            }

            return category;
        }

        private async Task<Category> GetSingleCategory(string categoryId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(categoryId);
            var category = await _categoryReadService.Categories.GetByIdAsync(categoryId);
            return category is null || !category.IsValid
                ? throw new NotFoundException("Kategori bulunamadı!")
                : category;
        }
    }
}
