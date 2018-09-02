using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Infrastructure.DatabaseAudit
{
    [Table("DbAuditTrail", Schema = "Audit")]
    public class DbAuditTrail
    {
        public string TableName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public long? TableIdValue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Actions { get; set; }
        public int Id { get; set; }
    }
}