using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Brands
{
    public class BrandRequestFilter : Pagination
    {
        public string Name { get; set; }
    }
}
