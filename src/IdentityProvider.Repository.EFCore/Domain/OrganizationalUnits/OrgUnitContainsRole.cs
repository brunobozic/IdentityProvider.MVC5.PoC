using IdentityProvider.Repository.EFCore.Domain.Roles;
using Module.CrossCutting;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits
{
    [Table("OrgUnitContainsRole", Schema = "Organization")]
    public class OrgUnitContainsRole : DomainEntity<int>, IActive, IFullAuditTrail
    {
        public OrgUnitContainsRole()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual OrganizationalUnit OrganizationalUnit { get; set; }
        public virtual AppRole Role { get; set; }

        public int? OrganizationalUnitId { get; set; }
        public Guid? RoleId { get; set; }

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