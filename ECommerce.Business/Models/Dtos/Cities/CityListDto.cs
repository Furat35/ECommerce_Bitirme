using ECommerce.Business.Models.Dtos.Districts;
using ECommerce.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Models.Dtos.Cities
{
    public class CityListDto
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public ICollection<DistrictListDto>? Districts { get; set; }
    }
}
