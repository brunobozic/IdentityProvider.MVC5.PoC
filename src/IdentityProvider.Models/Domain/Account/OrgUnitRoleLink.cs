using IdentityProvider.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("OrgUnitContainsRoleLink", Schema = "Organization")]
    public class OrgUnitContainsRoleLink : DomainEntity<int>, IActive
    {
        public OrgUnitContainsRoleLink()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual OrganizationalUnit OrganizationalUnit { get; set; }
        public virtual ApplicationRole Role { get; set; }

        public int OrganizationalUnitId { get; set; }
        public string RoleId { get; set; }

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
