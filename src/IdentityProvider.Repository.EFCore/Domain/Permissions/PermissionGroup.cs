using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using Module.CrossCutting;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Permissions
{
    [Table("PermissionGroup", Schema = "Resource")]
    public class PermissionGroup : DomainEntity<int>, IActive, IFullAuditTrail
    {
        public PermissionGroup()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(260,
            ErrorMessage =
                "The description of the resource permission group unit must be between 5 and 260 characters")]
        [MinLength(5)]
        public string Name { get; set; }

        [MaxLength(260,
            ErrorMessage = "The description of the resource permission group must be between 5 and 260 characters")]
        [MinLength(5)]
        public string Description { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        public ICollection<EmployeeOwnsPermissionGroup> Employees { get; set; }
        public ICollection<PermissionGroupOwnsPermission> Permissions { get; set; }
        public ICollection<RoleContainsPermissions> Roles { get; internal set; }

        #endregion IsActive
    }
}