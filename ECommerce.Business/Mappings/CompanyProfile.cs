using AutoMapper;
using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyAddDto, Company>();
            CreateMap<Company, CompanyListDto>();
        }
    }
}
