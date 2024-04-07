using ECommerce.Business.Models.Dtos.Users;

namespace ECommerce.Business.Models.Dtos.Companies
{
    public class CompanyListDto
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string AboutCompany { get; set; }
        public UserListDto User { get; set; }
    }
}
