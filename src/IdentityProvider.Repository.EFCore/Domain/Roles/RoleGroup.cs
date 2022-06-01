using IdentityProvider.Repository.EFCore.Domain.Account.Employees;
using IdentityProvider.Repository.EFCore.Domain.OrganizationalUnits;
using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace IdentityProvider.Repository.EFCore.Domain.Roles
{
    [Table("RoleGroup", Schema = "Organization")]
    public class RoleGroup : DomainEntity<int>, IActive
    {
        public RoleGroup()
        {
            OrganisationalUnits = new HashSet<OrgUnitContainsRoleGroup>();
            Roles = new HashSet<RoleGroupContainsRole>();
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

        public ICollection<RoleGroupContainsRole> Roles { get; set; }
        public ICollection<OrgUnitContainsRoleGroup> OrganisationalUnits { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        public List<OrgUnitContainsRoleGroup> FetchAssociatedOrganisationalUnits()
        {
            List<OrgUnitContainsRoleGroup> organisationalUnits;

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

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public ICollection<EmployeeOwnsRoleGroups> Employees { get; set; }

        #endregion IsActive
    }
}