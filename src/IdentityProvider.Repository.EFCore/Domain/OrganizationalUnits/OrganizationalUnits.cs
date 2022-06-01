using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits
{
    [Table("Unit", Schema = "Organization")]
    public class OrganizationalUnit : DomainEntity<int>, IActive
    {
        public OrganizationalUnit()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
            Employees = new HashSet<EmployeeBelongsToOrgUnit>();
            RoleGroups = new HashSet<OrgUnitContainsRoleGroup>();
            Roles = new HashSet<IdentityFrameworkRole>();
            SecurityWeight = 0; // Guest
        }

        [Required]
        [DisplayName("Name")]
        [MaxLength(50, ErrorMessage = "The name of the organizational unit must be between 2 and 50 characters")]
        [MinLength(2)]
        public string Name { get; set; }

        [DisplayName("Description")]
        [MaxLength(260,
            ErrorMessage = "The description of the organizational unit must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        [Required] [Range(0, 50)] public int SecurityWeight { get; set; }

        public ICollection<EmployeeBelongsToOrgUnit> Employees { get; set; }
        public ICollection<IdentityFrameworkRole> Roles { get; set; }
        public ICollection<OrgUnitContainsRoleGroup> RoleGroups { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        public List<EmployeeBelongsToOrgUnit> FetchAssociatedEmployees()
        {
            List<EmployeeBelongsToOrgUnit> employees;

            try
            {
                employees = Employees.Where(i => i.Active && !i.IsDeleted && i.EmployeeId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return employees;
        }

        public List<OrgUnitContainsRoleGroup> FetchAssociatedRoleGroups()
        {
            List<OrgUnitContainsRoleGroup> roleGroups;

            try
            {
                roleGroups = RoleGroups.Where(i => i.Active && !i.IsDeleted && i.RoleGroupId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return roleGroups;
        }

        #region IsActive

        public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}