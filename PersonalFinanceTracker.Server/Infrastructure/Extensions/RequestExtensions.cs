namespace PersonalFinanceTracker.Server.Infrastructure.Extensions
{
    using AutoFilter.Core;
    using Newtonsoft.Json;

    public static class RequestExtensions
    {
        public static List<Filter> GetFilters(this HttpRequest request)
        {
            List<Filter> filters = [];

            if (!string.IsNullOrWhiteSpace(request.Query["filters"]) && request.Query["filters"] != "[]")
            {
                filters = JsonConvert.DeserializeObject<List<Filter>>(request.Query["filters"]!) ?? [];
            }

            return filters;
        }
       
        public static List<Sort> GetSorters(this HttpRequest request)
        {
            List<Sort> sorters = [];

            if (!string.IsNullOrWhiteSpace(request.Query["sorters"]) && request.Query["sorters"] != "[]")
            {
                sorters = JsonConvert.DeserializeObject<List<Sort>>(request.Query["sorters"]!) ?? [];
            }

            return sorters;
        }
    }
}
