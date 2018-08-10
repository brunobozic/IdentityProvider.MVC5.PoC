using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using TrackableEntities;

namespace IdentityProvider.Infrastructure.Domain
{
    public abstract class DomainEntity<TK> : IFullAudit, ISoftDeletable, IHandlesConcurrency, ITrackable
    {
        public TK Id { get; set; }

        public Guid ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }
        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }

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

        public abstract IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext);

        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationErrors, true);
            return validationErrors;
        }
    }

    public abstract class ApplicationUserDomainEntity<TK> : IdentityUser, IFullAudit, ISoftDeletable,
        IHandlesConcurrency
    {
        private readonly List<BusinessRule> _brokenRules = new List<BusinessRule>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TK UserId { get; set; }

        public Guid? EditedById { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Checks if the current domain entity has an identity.
        /// </summary>
        /// <returns>
        ///     True if the domain entity is transient (i.e. has no identity yet),
        ///     false otherwise.
        /// </returns>
        public bool IsTransient()
        {
            return UserId.Equals(default(TK));
        }

        public override bool Equals(object entity)
        {
            return entity is ApplicationUserDomainEntity<TK> profile && this == profile;
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }

        public static bool operator ==(ApplicationUserDomainEntity<TK> entity1,
            ApplicationUserDomainEntity<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.UserId.ToString() == entity2.UserId.ToString())
                return true;

            return false;
        }

        public static bool operator !=(ApplicationUserDomainEntity<TK> entity1,
            ApplicationUserDomainEntity<TK> entity2)
        {
            return !(entity1 == entity2);
        }

        public abstract IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext);

        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationErrors, true);
            return validationErrors;
        }
    }
}