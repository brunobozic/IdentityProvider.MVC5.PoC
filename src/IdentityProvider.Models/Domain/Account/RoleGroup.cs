using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("RoleGroup", Schema = "Organization")]
    public class RoleGroup : DomainEntity<int>, IActive
    {
        public RoleGroup()
        {
            OrganisationalUnits = new HashSet<OrgUnitContainsRoleGroupLink>();
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(260, ErrorMessage = "The description of the role group unit must be between 5 and 260 characters")]
        [MinLength(5)]
        public string Name { get; set; }


        [MaxLength(260, ErrorMessage = "The description of the role group must be between 5 and 260 characters")]
        [MinLength(5)]
        public string Description { get; set; }

        public virtual ICollection<RoleGroupContainsRoleLink> Roles { get; set; }
        public virtual ICollection<OrgUnitContainsRoleGroupLink> OrganisationalUnits { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        public List<OrgUnitContainsRoleGroupLink> FetchAssociatedOrganisationalUnits()
        {
            List<OrgUnitContainsRoleGroupLink> organisationalUnits;

            try
            {
                organisationalUnits = OrganisationalUnits
                    .Where(i => i.Active && !i.IsDeleted && i.RoleGroupId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return organisationalUnits;
        }

        public List<RoleGroupContainsRoleLink> FetchAssociatedRoles()
        {
            List<RoleGroupContainsRoleLink> roles;

            try
            {
                roles = Roles.Where(i => i.Active && !i.IsDeleted && i.ApplicationRoleId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return roles;
        }

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}