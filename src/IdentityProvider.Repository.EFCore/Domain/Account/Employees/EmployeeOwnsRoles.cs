using IdentityProvider.Repository.EFCore.Domain.Roles;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Account.Employees
{
    [Table("EmployeeOwnsRoles", Schema = "Account")]
    public class EmployeeOwnsRoles : DomainEntity<int>, IActive
    {
        public EmployeeOwnsRoles()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Employee Employee { get; set; }
        public int? EmployeeId { get; set; }

        // Many to many with Roles (one RoleGroup will contain many Roles, one role can exist in many groups)
        public AppRole Role { get; set; }

        public string RoleId { get; set; }

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}