using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Linq;

namespace IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Extensions
{
    public static class SpecializedNameValueCollectionExtensions
    {
        public static string ConvertToJson(this NameValueCollection collection)
        {
            var returnValue = JsonConvert.SerializeObject(collection.AllKeys.ToDictionary(k => k, k => collection[k]));

            if (returnValue == "{}")
                returnValue = string.Empty;

            return returnValue;
        }
    }
}