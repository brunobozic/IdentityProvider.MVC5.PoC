using System;

namespace Module.CrossCutting.Models.ViewModels.Operations
{
    public class OperationAuditTrailDto
    {
        public int? Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string TableName { get; set; }
        public string UserName { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string TableId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Action { get; set; }
    }
}