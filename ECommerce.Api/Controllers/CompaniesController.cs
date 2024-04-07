using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.Consts;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    /// <summary>
    /// Şirket ile ilgili endpointleri içermektedir.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyReadService _companyReadService;
        private readonly ICompanyWriteService _companyWriteService;

        public CompaniesController(ICompanyReadService companyReadService, ICompanyWriteService companyWriteService)
        {
            _companyReadService = companyReadService;
            _companyWriteService = companyWriteService;
        }

        /// <summary>
        /// Verilen id'deki şirket getirilmektedir
        /// </summary>
        /// <param name="companyId">Şirket id'si</param>
        /// <returns>Şirket bilgisi</returns>
        [HttpGet("{companyId}")]
        [Authorize(Roles = $"{RoleConsts.Admin},{RoleConsts.Company}")]
        public async Task<IActionResult> GetCompanyById(string companyId)
        {
            string userId = HttpContext.User.GetActiveUserId();
            if (!(_companyReadService.CheckIfCompanyBelongsToUser(userId, companyId) || HttpContext.User.IsInRole(RoleConsts.Admin)))
                throw new ForbiddenException();
            var company = await _companyReadService.GetCompanyById(companyId);

            return Ok(company);
        }

        /// <summary>
        /// Şirket bilgileri güncelleme
        /// </summary>
        /// <param name="companyUpdate">Yeni şirket bilgileri</param>
        /// <returns>İşlemin başarılı olup olmadığı</returns>
        [HttpPut]
        [Authorize(Roles = $"{RoleConsts.Company}")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyUpdateDto companyUpdate)
        {
            string userId = HttpContext.User.GetActiveUserId();
            if (!_companyReadService.CheckIfCompanyBelongsToUser(userId, companyUpdate.Id))
                throw new ForbiddenException();
            bool isUpdated = await _companyWriteService.UpdateCompany(companyUpdate);

            return Ok(isUpdated);
        }
    }
}
