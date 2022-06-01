using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities.Common.Core;

namespace IdentityProvider.Repository.EFCore.Domain
{
    [Table("DbAuditTrail", Schema = "Audit")]
    public class DbAuditTrail : ITrackable, ISoftDeletable
    {
        public string TableName { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public long? TableIdValue { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Actions { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }
    }
}