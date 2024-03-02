using IdentityProvider.Repository.EFCore.Domain.Roles;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Account.Employees
{
    [Table("RoleGroups", Schema = "Account")]
    public class EmployeeOwnsRoleGroups : DomainEntity<int>, IActive
    {
        public EmployeeOwnsRoleGroups()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public Employee Employee { get; set; }
        public RoleGroup RoleGroup { get; set; }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public Guid EmployeeId { get; set; }
        public int RoleGroupId { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}