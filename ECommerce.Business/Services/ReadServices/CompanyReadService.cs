using AutoMapper;
using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.ReadServices
{
    public class CompanyReadService : ICompanyReadService
    {
        private readonly IMapper _mapper;

        public CompanyReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            Companies = unitOfWork.GetReadRepository<Company>();
            _mapper = mapper;
        }

        public IReadRepository<Company> Companies { get; }

        public async Task<CompanyListDto> GetCompanyById(string companyId)
        {
            var company = await Companies.GetByIdAsync(companyId, false, [_ => _.User]);
            if (company is null)
                throw new NotFoundException("Şirkte bulunamadı!");

            return _mapper.Map<CompanyListDto>(company);
        }

        public bool CheckIfCompanyBelongsToUser(string userId, string companyId)
            => userId.Equals(companyId, StringComparison.InvariantCultureIgnoreCase);
    }
}
