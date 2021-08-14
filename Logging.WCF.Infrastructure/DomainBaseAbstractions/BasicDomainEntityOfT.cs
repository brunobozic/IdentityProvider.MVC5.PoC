using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Logging.WCF.Infrastructure.BusinessRules;
using TrackableEntities;

namespace Logging.WCF.Infrastructure.DomainBaseAbstractions
{
    public abstract class BasicDomainEntity<TK> : ITrackable
    {
        public TK Id { get; set; }

        public override bool Equals(object entity)
        {
            return entity is BasicDomainEntity<TK> @base && this == @base;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(BasicDomainEntity<TK> entity1,
            BasicDomainEntity<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.Id.ToString() == entity2.Id.ToString())
                return true;

            return false;
        }

        public static bool operator !=(BasicDomainEntity<TK> entity1,
            BasicDomainEntity<TK> entity2)
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

        #region ITrackable

        [NotMapped] public TrackingState TrackingState { get; set; }

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }

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