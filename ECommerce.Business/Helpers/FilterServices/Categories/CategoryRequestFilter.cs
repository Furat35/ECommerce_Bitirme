using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Categories
{
    public class CategoryRequestFilter : Pagination
    {
        public string Name { get; set; }
    }
}
