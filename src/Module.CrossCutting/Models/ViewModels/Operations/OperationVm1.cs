namespace Module.CrossCutting.Models.ViewModels.Operations
{
    public class OperationVm
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public OperationDto Operation { get; set; } = new OperationDto();
    }
}