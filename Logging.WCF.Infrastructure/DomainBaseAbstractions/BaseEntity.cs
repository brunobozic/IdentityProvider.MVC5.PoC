using Logging.WCF.Repository.EF.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities;

namespace Logging.WCF.Repository.EF.DomainBaseAbstractions
{
    public class BaseEntity : IEntityBase, ISoftDeletable, IDeactivatableEntity, ICreationAuditedEntity,
        IModificationAuditedEntity, IDeletionAuditedEntity, ITrackable
    {
        public BaseEntity()
        {
            Deleted = false;
            IsActive = true;
        }

        [Column("Name", Order = 1)] public string Name { get; set; }

        [Column("Description", Order = 2)] public string Description { get; set; }

        public long CreatedBy { get; set; }

        [Required]
        [Column("IsActive", Order = 990)]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("ActiveFrom", Order = 991)]
        public DateTimeOffset ActiveFrom { get; set; } = DateTimeOffset.UtcNow;

        [Column("ActiveTo", Order = 992)] public DateTimeOffset? ActiveTo { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("DateCreated", Order = 1005)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Column("DateModified", Order = 1010)] public DateTimeOffset? DateModified { get; set; }

        [Column("DateDeleted", Order = 1020)] public DateTimeOffset? DateDeleted { get; set; }

        public long ModifiedBy { get; set; }

        [Required]
        [Column("Deleted", Order = 996)]
        public bool Deleted { get; set; }

        public long DeletedBy { get; set; }

        [NotMapped] public TrackingState TrackingState { get; set; }

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }


    }
}