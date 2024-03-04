using ECommerce.Core.Filters;
using System.Text.Json;

namespace ECommerce.Core.ResponseHeaders
{
    public class CustomHeaders
    {
        public Dictionary<string, string> AddPaginationHeader(Metadata metadata)
            => metadata != null
            ? new Dictionary<string, string>() { { "X-Pagination", JsonSerializer.Serialize(metadata) } }
            : throw new Exception();
    }
}
