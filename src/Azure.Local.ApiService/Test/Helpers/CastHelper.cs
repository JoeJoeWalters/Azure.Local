using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.Domain.Test;

namespace Azure.Local.ApiService.Test.Helpers
{
    public static class CastHelper
    {
        public static TestItem ToTestItem(this AddTestItemHttpRequest request)
        {
            return new TestItem
            {
                Id = request.Id,
                Name = request.Name
            };
        }
    }
}
