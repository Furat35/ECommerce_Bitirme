using AutoMapper;
using ECommerce.Business.Models.Dtos.Districts;
using ECommerce.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Mappings
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<District, DistrictListDto>();
        }
    }
}
