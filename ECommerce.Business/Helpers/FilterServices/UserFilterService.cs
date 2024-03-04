using AutoMapper;
using ECommerce.Business.Helpers.Users;
using ECommerce.Business.Models.Dtos.Users;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class UserFilterService
    {
        private readonly IMapper _mapper;
        private IQueryable<User> _users;

        public UserFilterService(IMapper mapper, IQueryable<User> users)
        {
            _mapper = mapper;
            _users = users;
        }

        public UserResponse<List<UserListDto>> FilterUsers(UserRequestFilter filters)
        {
            _users = NameStartsWith(filters.Name);
            _users = MailStartsWith(filters.Mail);

            Metadata metadata = new(filters.Page, filters.PageSize, _users.Count(), _users.Count() / filters.PageSize + 1);
            _users = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedProducts = _mapper.Map<List<UserListDto>>(_users);

            return new()
            {
                ResponseValue = mappedProducts,
                Headers = header
            };
        }

        private IQueryable<User> NameStartsWith(string name)
           => !string.IsNullOrEmpty(name)
              ? _users.Where(product => product.Name.StartsWith(name))
              : _users;

        private IQueryable<User> MailStartsWith(string mail)
           => !string.IsNullOrEmpty(mail)
              ? _users.Where(product => product.Mail.StartsWith(mail))
              : _users;

        private IQueryable<User> AddPagination(UserRequestFilter filters)
           => _users
               .OrderBy(_ => _.Name)
               .Skip(filters.Page * filters.PageSize)
               .Take(filters.PageSize);
    }
}
