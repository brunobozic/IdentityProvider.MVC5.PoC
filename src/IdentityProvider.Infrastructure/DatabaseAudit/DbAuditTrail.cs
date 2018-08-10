using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Infrastructure.DatabaseAudit
{
    [Table("DbAuditTrail", Schema = "Audit")]
    public class DbAuditTrail : DomainEntity<int>
    {
        public string TableName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public long? TableIdValue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Actions { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}