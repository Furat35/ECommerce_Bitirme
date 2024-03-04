using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Products
{
    public class ProductResponse<T> : ResponseFilter<T> where T : class, new()
    {

    }
}
