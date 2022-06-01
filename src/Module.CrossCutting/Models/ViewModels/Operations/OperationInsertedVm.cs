namespace Module.CrossCutting.Models.ViewModels.Operations
{
    public class OperationInsertedVm
    {
        public int WasInserted { get; set; }
        public bool Success { get; set; }
        public string ValidationIssues { get; set; }
        public string Message { get; set; }
        public object FormErrors { get; set; }
    }
}