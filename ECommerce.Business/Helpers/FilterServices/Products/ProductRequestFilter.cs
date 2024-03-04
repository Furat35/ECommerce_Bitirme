using ECommerce.Core.Filters;

namespace ECommerce.Business.Helpers.Products
{
    public class ProductRequestFilter : Pagination
    {
        public string? ProductName { get; set; }
        public float? GreaterThan { get; set; }
        public float? LessThan { get; set; }
        public Guid? SubCategory { get; set; }
        public string OrderBy { get; set; } = "productname";
        public string Order { get; set; } = "asc";
    }
}
