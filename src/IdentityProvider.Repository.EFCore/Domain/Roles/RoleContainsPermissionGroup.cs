using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    [Table("RoleContainsPermissionGroup", Schema = "Organization")]
    public class RoleContainsPermissionGroup : DomainEntity<int>, IActive
    {
        public RoleContainsPermissionGroup()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual AppRole Role { get; set; }
        public virtual PermissionGroup PermissionGroup { get; set; }

        public string RoleId { get; set; }
        public int PermissionGroupId { get; set; }

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