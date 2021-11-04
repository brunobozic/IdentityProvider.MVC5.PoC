namespace IdentityProvider.Models.ViewModels.Operations
{
    public class OperationVm
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OperationDto Operation { get; set; }
        public object FormErrors { get; set; }
    }
}