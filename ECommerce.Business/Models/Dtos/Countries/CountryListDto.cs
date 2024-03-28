using ECommerce.Business.Models.Dtos.Cities;
using ECommerce.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Models.Dtos.Countries
{
    public class CountryListDto
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
        public ICollection<CityListDto>? Cities { get; set; }
    }
}
