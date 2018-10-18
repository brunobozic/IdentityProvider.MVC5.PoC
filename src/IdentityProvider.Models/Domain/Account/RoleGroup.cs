using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("RoleGroups" , Schema = "Account")]
    public class RoleGroup : DomainEntity<int>, IActive
    {
        public RoleGroup()
        {
          
            OrganisationalUnits = new HashSet<OrgUnitRoleGroupJoin>();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(260 , ErrorMessage = "The description of the role group unit must be between 5 and 260 characters"), MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(260 , ErrorMessage = "The description of the role group must be between 5 and 260 characters"), MinLength(5)]
        public string Description { get; set; }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        public virtual ICollection<RoleGroupRoleJoin> Roles { get; set; }
        public virtual ICollection<OrgUnitRoleGroupJoin> OrganisationalUnits { get; set; }

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            return null;
        }

        public List<OrgUnitRoleGroupJoin> FetchAssociatedOrganisationalUnits()
        {
            List<OrgUnitRoleGroupJoin> organisationalUnits;

            try
            {
                organisationalUnits = OrganisationalUnits.Where(i => i.Active && !i.IsDeleted && i.RoleGroupId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return organisationalUnits;
        }

        public List<RoleGroupRoleJoin> FetchAssociatedRoles()
        {
            List<RoleGroupRoleJoin> roles;

            try
            {
                roles = Roles.Where(i => i.Active && !i.IsDeleted && i.RoleId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return roles;
        }
    }
}
