using IdentityProvider.Repository.EFCore.Domain.Permissions;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Account.Employees
{
    [Table("EmployeeOwnsPermissionGroup", Schema = "Organization")]
    public class EmployeeOwnsPermissionGroup : DomainEntity<int>, IActive
    {
        public EmployeeOwnsPermissionGroup()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public PermissionGroup PermissionGroup { get; set; }
        public Employee Employee { get; set; }

        public Guid EmployeeId { get; set; }

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

        public int PermissionGroupId { get; set; }

        #endregion IsActive
    }
}