
namespace IdentityProvider.Models.ViewModels.Operations
{

    public class OperationCountsDto
    {
        public int ActiveItemCount { get; set; }
        public int DeletedItemCount { get; set; }
        public int InactiveItemCount { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
}
