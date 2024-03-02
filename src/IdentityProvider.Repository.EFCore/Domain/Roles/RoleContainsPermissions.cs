using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    [Table("RoleContainsGroup", Schema = "Organization")]
    public class RoleContainsPermissions : DomainEntity<int>, IActive
    {
        public RoleContainsPermissions()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }
        [ForeignKey("RoleId")]
        public AppRole Role { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey("PermissionGroupId")]
        public PermissionGroup PermissionGroup { get; set; }
        public int? PermissionGroupId { get; set; }
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }
        public int? PermissionId { get; set; }

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