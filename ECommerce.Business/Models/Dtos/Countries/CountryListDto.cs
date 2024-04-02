using ECommerce.Business.Models.Dtos.Cities;

namespace ECommerce.Business.Models.Dtos.Countries
{
    public class CountryListDto
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
        public ICollection<CityListDto>? Cities { get; set; }
    }
}
