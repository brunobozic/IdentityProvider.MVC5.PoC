namespace Module.CrossCutting.Models.ViewModels.Operations
{
    public class OperationAuditTrailVm
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OperationAuditTrailDto OperationAuditTrail { get; set; }
        public object FormErrors { get; set; }
    }
}