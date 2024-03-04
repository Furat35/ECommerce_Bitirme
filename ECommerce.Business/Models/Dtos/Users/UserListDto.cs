using ECommerce.Business.Models.Dtos.Companies;

namespace ECommerce.Business.Models.Dtos.Users
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public CompanyListDto? Company { get; set; }
        public bool IsValid { get; set; }
    }
}
