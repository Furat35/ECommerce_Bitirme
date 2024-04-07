using ECommerce.Business.Models.Dtos.Companies;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface ICompanyWriteService
    {
        Task<bool> UpdateCompany(CompanyUpdateDto companyUpdate);
    }
}
