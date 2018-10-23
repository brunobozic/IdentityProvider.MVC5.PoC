using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("Unit" , Schema = "Organization")]
    public class OrganizationalUnit : DomainEntity<int>, IActive
    {
        public OrganizationalUnit()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;

            SecurityWeight = 0; // Guest
        }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive

        [Required]
        [MaxLength(50 , ErrorMessage = "The name of the organizational unit must be between 2 and 50 characters"), MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(260 , ErrorMessage = "The description of the organizational unit must be between 2 and 260 characters"), MinLength(2)]
        public string Description { get; set; }

        [Required]
        [Range(0 , 50)]
        public int SecurityWeight { get; set; }

        public ICollection<EmployeeBelongsToOrgUnitLink> Employees { get; set; }
        public ICollection<OrgUnitContainsRoleGroupLink> RoleGroups { get; set; }
        public ICollection<OrgUnitContainsRoleGroupLink> OrganisationalUnits { get; set; }

        public List<EmployeeBelongsToOrgUnitLink> FetchAssociatedEmployees()
        {
            List<EmployeeBelongsToOrgUnitLink> employees;

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

        public List<OrgUnitContainsRoleGroupLink> FetchAssociatedRoleGroups()
        {
            List<OrgUnitContainsRoleGroupLink> roleGroups;

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
    }
}
