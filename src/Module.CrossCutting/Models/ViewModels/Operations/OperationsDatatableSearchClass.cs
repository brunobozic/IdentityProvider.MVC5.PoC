using Newtonsoft.Json;
using System.Text.Json;

namespace Module.CrossCutting.Models.ViewModels.Operations
{
    [Serializable]
    public class OperationsDatatableSearchClass
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("description")] public string? Description { get; set; }
        [JsonProperty("active")] public bool Active { get; set; }
        [JsonProperty("createdDate")] public DateTime? CreatedDate { get; set; }
        [JsonProperty("modifiedDate")] public DateTime? ModifiedDate { get; set; }
        [JsonProperty("actions")] public string Actions { get; set; }
        [JsonProperty("deleted")] public bool Deleted { get; set; }
    }
}