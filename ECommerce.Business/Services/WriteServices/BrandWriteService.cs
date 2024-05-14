using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.Brands;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using FluentValidation;

namespace ECommerce.Business.Services.WriteServices
{
    public class BrandWriteService : IBrandWriteService
    {
        private readonly IWriteRepository<Brand> _brandWriteRepository;
        private readonly IBrandReadService _brandReadService;
        private readonly IMapper _mapper;
        private readonly IValidator<BrandAddDto> _brandAddDtoValidator;
        private readonly IValidator<BrandUpdateDto> _brandUpdateDtoValidator;

        public BrandWriteService(IUnitOfWork unitOfWork, IMapper mapper, IBrandReadService brandReadService, IValidator<BrandAddDto> brandAddDtoValidator, IValidator<BrandUpdateDto> brandUpdateDtoValidator)
        {
            _brandWriteRepository = unitOfWork.GetWriteRepository<Brand>();
            _mapper = mapper;
            _brandReadService = brandReadService;
            _brandAddDtoValidator = brandAddDtoValidator;
            _brandUpdateDtoValidator = brandUpdateDtoValidator;
        }

        public async Task<BrandListDto> AddBrandAsync(BrandAddDto brand)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(brand, _brandAddDtoValidator);
            await ActivateBrandIfDeleted(brand.Name);
            var brandToAdd = _mapper.Map<Brand>(brand);
            brandToAdd.IsValid = true;
            bool isAdded = await _brandWriteRepository.AddAsync(brandToAdd);
            if (!isAdded)
                throw new InternalServerErrorException();
            var brandToList = _mapper.Map<BrandListDto>(brandToAdd);

            return brandToList;
        }

        public async Task<bool> SafeRemoveBrandAsync(string brandId)
        {
            var brand = await GetSingleBrand(brandId);
            return await _brandWriteRepository.SafeRemoveAsync(brand);
        }

        public async Task<bool> UpdateBrandAsync(BrandUpdateDto brand)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(brand, _brandUpdateDtoValidator);
            var brandToUpdate = await GetSingleBrand(brand.Id);
            _mapper.Map(brand, brandToUpdate);

            return await _brandWriteRepository.UpdateAsync(brandToUpdate);
        }

        private async Task<Brand> GetSingleBrand(string brandId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(brandId);
            var brand = await _brandReadService.Brands.GetByIdAsync(brandId);
            if (brand is null || !brand.IsValid)
                throw new NotFoundException("Marka bulunamadı!");

            return brand;
        }

        private async Task ActivateBrandIfDeleted(string name)
        {
            var brand = await _brandReadService.Brands.GetSingleAsync(_ => _.Name == name);
            if (brand != null)
            {
                if (!brand.IsValid)
                {
                    brand.DeletedDate = DateTime.MinValue;
                    brand.IsValid = true;
                    await _brandWriteRepository.UpdateAsync(brand);
                    return;
                }

                throw new BadRequestException("Marka mevcut!");
            }
        }
    }
}
