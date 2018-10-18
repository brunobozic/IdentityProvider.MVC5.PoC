using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    public class OrganisationalUnit : DomainEntity<int>, IActive
    {
        public OrganisationalUnit()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        
            SecurityWeight = 0; // Guest
        }

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        [Required]
        [MaxLength(50 , ErrorMessage = "The name of the organizational unit must be between 20 and 50 characters"), MinLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(260 , ErrorMessage = "The description of the organizational unit must be between 5 and 260 characters"), MinLength(5)]
        public string Description { get; set; }

        [Required]
        [Range(0 , 50)]
        public int SecurityWeight { get; set; }

        public ICollection<EmployeeOrgUnitJoin> Employees { get; set; }
        public ICollection<OrgUnitRoleGroupJoin> RoleGroups { get; set; }
        public ICollection<OrgUnitRoleGroupJoin> OrganisationalUnits { get; set; }

        public List<EmployeeOrgUnitJoin> FetchAssociatedEmployees()
        {
            List<EmployeeOrgUnitJoin> employees;

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

        public List<OrgUnitRoleGroupJoin> FetchAssociatedRoleGroups()
        {
            List<OrgUnitRoleGroupJoin> roleGroups;

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
