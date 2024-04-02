using ECommerce.Business.Models.Dtos.Districts;

namespace ECommerce.Business.Models.Dtos.Cities
{
    public class CityListDto
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public ICollection<DistrictListDto>? Districts { get; set; }
    }
}
