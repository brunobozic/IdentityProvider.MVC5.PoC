using Newtonsoft.Json;

namespace Module.CrossCutting.Models.Datatables
{
    [Serializable]
    public class DataTableAjaxPostModel
    {
        [JsonProperty] public bool alsoinactive { get; set; } = false;
        [JsonProperty] public bool alsodeleted { get; set; } = false;
        [JsonProperty] public List<string> tables { get; set; }
        [JsonProperty] public List<string> actions { get; set; }
        [JsonProperty] public int userid { get; set; } = 1;
        [JsonProperty] public int draw { get; set; }
        [JsonProperty] public int start { get; set; } = 1;
        [JsonProperty] public int length { get; set; } = 10;
        [JsonProperty] public DateTime? from { get; set; } = DateTime.UtcNow.AddYears(-1);
        [JsonProperty] public DateTime? to { get; set; } = DateTime.UtcNow.AddDays(1);
        [JsonProperty] public List<Column> columns { get; set; }
        [JsonProperty] public Search search { get; set; }
        [JsonProperty] public string search_extra { get; set; }
        [JsonProperty] public string search_userName { get; set; }
        [JsonProperty] public string search_oldValue { get; set; }
        [JsonProperty] public string search_newValue { get; set; }
        [JsonProperty] public List<Order> order { get; set; }
    }
}