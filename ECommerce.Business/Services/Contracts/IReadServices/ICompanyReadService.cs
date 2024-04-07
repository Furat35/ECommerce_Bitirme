using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface ICompanyReadService
    {
        IReadRepository<Company> Companies { get; }
        Task<CompanyListDto> GetCompanyById(string companyId);
        bool CheckIfCompanyBelongsToUser(string userId, string companyId);
    }
}
