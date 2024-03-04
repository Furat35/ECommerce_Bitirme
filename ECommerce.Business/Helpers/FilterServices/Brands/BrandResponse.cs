using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Brands
{
    public class BrandResponse<T> : ResponseFilter<T> where T : class, new()
    {
    }
}
