using AutoMapper;
using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Business.Models.Dtos.Cities;
using ECommerce.Business.Models.Dtos.Countries;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.ReadServices
{
    public class AddressReadService : IAddressReadService
    {
        private readonly IMapper _mapper;

        public AddressReadService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            Addresses = unitOfWork.GetReadRepository<Address>();
            Countries = unitOfWork.GetReadRepository<Country>();
            Cities = unitOfWork.GetReadRepository<City>();
            Districts = unitOfWork.GetReadRepository<District>();
            _mapper = mapper;
        }

        public IReadRepository<Address> Addresses { get; }
        public IReadRepository<Country> Countries { get; }
        public IReadRepository<City> Cities { get; }
        public IReadRepository<District> Districts { get; }

        public async Task<AddressListDto> GetUserAddress(string userId)
        {
            var address = await Addresses.GetSingleAsync(_ => _.UserId == Guid.Parse(userId));
            return address != null ? _mapper.Map<AddressListDto>(address) : null;
        }

        public CountryListDto GetCountryById(string countryId)
        {
            var country = Countries.GetWhere(_ => _.Id == Guid.Parse(countryId)).FirstOrDefault();
            return country != null ? _mapper.Map<CountryListDto>(country) : null;
        }

        public List<CountryListDto> GetCountries()
        {
            return _mapper.Map<List<CountryListDto>>(Countries.GetAll());
        }

        public CountryListDto GetCitiesByCountryId(string countryId)
        {
            var cities = Countries.GetWhere(_ => _.Id == Guid.Parse(countryId), false, _ => _.Cities).FirstOrDefault();
            return cities != null ? _mapper.Map<CountryListDto>(cities) : null;
        }

        public CityListDto GetDistrictsByCityId(string cityId)
        {
            var districts = Cities.GetWhere(_ => _.Id == Guid.Parse(cityId), false, _ => _.Districts).FirstOrDefault();
            return districts != null ? _mapper.Map<CityListDto>(districts) : null;
        }
    }
}
