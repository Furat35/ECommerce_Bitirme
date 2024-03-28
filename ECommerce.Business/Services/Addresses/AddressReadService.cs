using AutoMapper;
using ECommerce.Business.Models.Dtos.Cities;
using ECommerce.Business.Models.Dtos.Countries;
using ECommerce.Business.Models.Dtos.Districts;
using ECommerce.Business.Services.Addresses.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services.Addresses
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

        public CountryListDto GetCountryById(string countryId)
        {
            var country = Countries.GetWhere(_ => _.Id == Guid.Parse(countryId)).FirstOrDefault();
            return _mapper.Map<CountryListDto>(country);
        }
        
        public List<CountryListDto> GetCountries()
        {
            return _mapper.Map<List<CountryListDto>>(Countries.GetAll());
        }

        public CountryListDto GetCitiesByCountryId(string countryId)
        {
            var cities = Countries.GetWhere(_ => _.Id == Guid.Parse(countryId), false, _ => _.Cities).FirstOrDefault();
            return _mapper.Map<CountryListDto>(cities);
        }

        public CityListDto GetDistrictsByCityId(string cityId)
        {
            var districts = Cities.GetWhere(_ => _.Id == Guid.Parse(cityId), false, _ => _.Districts).FirstOrDefault();
            return _mapper.Map<CityListDto>(districts);
        }   
    }
}
