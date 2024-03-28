using AutoMapper;
using ECommerce.Business.Models.Dtos.Addresses;
using ECommerce.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressUpdateDto, Address>();
        }
    }
}
