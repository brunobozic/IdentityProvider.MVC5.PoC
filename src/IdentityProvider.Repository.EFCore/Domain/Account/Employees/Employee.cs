using Microsoft.AspNetCore.Identity;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace IdentityProvider.Repository.EFCore.Domain.Account.Employees
{
    [Table("Employee", Schema = "Organization")]
    public class Employee : DomainEntity<int>, IActive
    {
        public Employee()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
            OrganizationalUnits = new HashSet<EmployeeBelongsToOrgUnit>();
            RoleGroups = new HashSet<EmployeeOwnsRoleGroups>();
            Roles = new HashSet<EmployeeOwnsRoles>();
        }

        // Employee can belong to many Org units
        public ICollection<EmployeeBelongsToOrgUnit> OrganizationalUnits { get; set; }

        public ICollection<EmployeeOwnsRoleGroups> RoleGroups { get; set; }
        public ICollection<EmployeeOwnsRoles> Roles { get; set; }

        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Surname { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        public List<EmployeeBelongsToOrgUnit> FetchAssociatedOrganisationalUnits()
        {
            List<EmployeeBelongsToOrgUnit> organisationalUnits;

            try
            {
                organisationalUnits = OrganizationalUnits
                    .Where(i => i.Active && !i.IsDeleted && i.EmployeeId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return organisationalUnits;
        }

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        public ICollection<EmployeeOwnsPermissionGroup> PermissionGroups { get; set; }

        #endregion IsActive
    }
}