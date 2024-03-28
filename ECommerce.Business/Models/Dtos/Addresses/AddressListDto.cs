using ECommerce.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Models.Dtos.Addresses
{
    public class AddressListDto
    {
        public Guid Id { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Guid DistrictId { get; set; }
    }
}
