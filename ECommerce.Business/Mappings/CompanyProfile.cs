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
            CreateMap<CompanyUpdateDto, Company>().ForMember(_ => _.Id, opt => opt.Ignore());
        }
    }
}
