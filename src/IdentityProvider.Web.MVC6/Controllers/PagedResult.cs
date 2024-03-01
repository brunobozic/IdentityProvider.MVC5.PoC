using System.Collections.Generic;

namespace IdentityProvider.Web.MVC6.Controllers
{
    public class PagedResult<T>
    {
        public int Count { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}