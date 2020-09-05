using IdentityProvider.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("RoleGroupContainsRoleLink", Schema = "Organization")]
    public class RoleGroupContainsRoleLink : DomainEntity<int>, IActive
    {
        public RoleGroupContainsRoleLink()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual ApplicationRole Role { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }

        // public string RoleId { get; set; }
        public string ApplicationRoleId { get; set; }
        public int RoleGroupId { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}
