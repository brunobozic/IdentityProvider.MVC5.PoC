using System;
using System.Collections.Generic;

namespace IdentityProvider.Models.Datatables
{
    public class DataTableAjaxPostModel
    {
        // properties are not capital due to json mapping
        public int userid { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
    }
}
