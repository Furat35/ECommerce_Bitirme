using ECommerce.Business.Models.Dtos.Companies;

namespace ECommerce.Business.Models.Dtos.Auth
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public CompanyAddDto Company { get; set; }
    }
}
