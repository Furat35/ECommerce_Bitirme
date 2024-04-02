using AutoMapper;
using ECommerce.Business.Models.Dtos.InvoiceInfos;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class InvoiceInfoProfile : Profile
    {
        public InvoiceInfoProfile()
        {
            CreateMap<InvoiceInfoCheckoutDto, InvoiceInfo>();
        }
    }
}
