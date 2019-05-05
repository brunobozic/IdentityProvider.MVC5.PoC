namespace IdentityProvider.Models.ViewModels.Resources
{
    public class ResourceVm
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResourceDto Resource { get; set; }
        public object FormErrors { get; set; }
    }
}
