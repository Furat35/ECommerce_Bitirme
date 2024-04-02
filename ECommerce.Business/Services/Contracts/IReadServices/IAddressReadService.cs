using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Business.Models.Dtos.Cities;
using ECommerce.Business.Models.Dtos.Countries;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface IAddressReadService
    {
        Task<AddressListDto> GetUserAddress(string userId);
        CountryListDto GetCountryById(string countryId);
        List<CountryListDto> GetCountries();
        CountryListDto GetCitiesByCountryId(string countryId);
        CityListDto GetDistrictsByCityId(string cityId);
        IReadRepository<Address> Addresses { get; }
    }
}
