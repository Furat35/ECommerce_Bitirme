using ECommerce.Business.Models.Dtos.Companies;
using ECommerce.Entity.Enums;

namespace ECommerce.Business.Models.Dtos.Users
{
    public class UserAddDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public CompanyAddDto Company { get; set; }
    }
}
