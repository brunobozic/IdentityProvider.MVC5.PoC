namespace IdentityProvider.Models.ViewModels.Resources
{
    public class ResourceInsertedVm
    {
        public int WasInserted { get; set; }
        public bool Success { get; set; }
        public string ValidationIssues { get; set; }
        public string Message { get; set; }
    }
}