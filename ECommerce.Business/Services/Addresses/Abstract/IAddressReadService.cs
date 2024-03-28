using ECommerce.Business.Models.Dtos.Cities;
using ECommerce.Business.Models.Dtos.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services.Addresses.Abstract
{
    public interface IAddressReadService
    {
        CountryListDto GetCountryById(string countryId);
        List<CountryListDto> GetCountries();
        CountryListDto GetCitiesByCountryId(string countryId);
        CityListDto GetDistrictsByCityId(string cityId);
    }
}
