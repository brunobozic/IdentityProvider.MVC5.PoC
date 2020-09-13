using Logging.WCF.Repository.EF.BusinessRules;
using Logging.WCF.Repository.EF.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using TrackableEntities;

namespace Logging.WCF.Repository.EF.DomainBaseAbstractions
{
    public abstract class DomainEntity<TK> : IHandlesConcurrency, ISoftDeletable, IDeactivatableEntity,
        ICreationAuditedEntity, IModificationAuditedEntity, IDeletionAuditedEntity, ITrackable
    {
        public TK Id { get; set; }

        [Required] [Column("Name", Order = 1)] public string Name { get; set; }

        [Column("Description", Order = 2)] public string Description { get; set; }

        #region ICreationAuditedEntity

        [Required]
        [Column("DateCreated", Order = 1005)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        #endregion ICreationAuditedEntity

        #region IHandlesConcurrency

        public byte[] RowVersion { get; set; }

        #endregion IHandlesConcurrency

        #region IModificationAuditedEntity

        [Column("DateModified", Order = 1010)] public DateTimeOffset? DateModified { get; set; }

        #endregion IModificationAuditedEntity

        #region IDeletionAuditedEntity

        [Column("DateDeleted", Order = 1020)] public DateTimeOffset? DateDeleted { get; set; }

        #endregion IDeletionAuditedEntity

        #region ISoftDeletable

        [Required]
        [Column("Deleted", Order = 996)]
        public bool Deleted { get; set; } = false;

        #endregion ISoftDeletable

        public override bool Equals(object entity)
        {
            return entity is DomainEntity<TK> @base && this == @base;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(DomainEntity<TK> entity1,
            DomainEntity<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.Id.ToString() == entity2.Id.ToString())
                return true;

            return false;
        }

        public static bool operator !=(DomainEntity<TK> entity1,
            DomainEntity<TK> entity2)
        {
            return !(entity1 == entity2);
        }

        /// <summary>
        ///     Checks if the current domain entity has an identity.
        /// </summary>
        /// <returns>
        ///     True if the domain entity is transient (i.e. has no identity yet),
        ///     false otherwise.
        /// </returns>
        public bool IsTransient()
        {
            return Id.Equals(default(TK));
        }

        #region IDeactivatableEntity

        [Required]
        [Column("IsActive", Order = 990)]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("ActiveFrom", Order = 991)]
        public DateTimeOffset ActiveFrom { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        [Column("ActiveTo", Order = 992)]
        public DateTimeOffset? ActiveTo { get; set; }

        #endregion IDeactivatableEntity

        #region ITrackable

        [NotMapped] public TrackingState TrackingState { get; set; }

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }
        public long DeletedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long ModifiedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion ITrackable

        #region Business rules

        private readonly List<BusinessRule> _brokenRules = new List<BusinessRule>();

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }

        public void ThrowExceptionIfInvalid()
        {
            _brokenRules.Clear();

            Validate();

            if (_brokenRules.Any())
            {
                var issues = new StringBuilder();

                foreach (var businessRule in _brokenRules)
                    issues.AppendLine(businessRule.Rule);

                throw new EntityIsInvalidException(issues.ToString());
            }
        }

        public abstract IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext
        );

        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();

            var ctx = new ValidationContext(this, null, null);

            Validator.TryValidateObject(this, ctx, validationErrors, true);

            return validationErrors;
        }

        #endregion Business rules

    }
}