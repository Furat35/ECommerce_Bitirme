using AutoMapper;
using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.WriteServices
{
    public class CompanyWriteService : ICompanyWriteService
    {
        private readonly IMapper _mapper;
        private readonly IWriteRepository<Company> _companyWriteRepository;
        private readonly ICompanyReadService _companyReadService;

        public CompanyWriteService(IUnitOfWork unitOfWork, IMapper mapper, ICompanyReadService companyReadService)
        {
            _companyWriteRepository = unitOfWork.GetWriteRepository<Company>();
            _mapper = mapper;
            _companyReadService = companyReadService;
        }

        public async Task<bool> UpdateCompany(CompanyUpdateDto companyUpdate)
        {
            var company = await _companyReadService.Companies.GetByIdAsync(companyUpdate.Id);
            if (company is null)
                throw new NotFoundException("Şirkte bulunamadı!");
            company.IsValid = true;
            _mapper.Map(companyUpdate, company);

            return await _companyWriteRepository.UpdateAsync(company);
        }
    }
}
