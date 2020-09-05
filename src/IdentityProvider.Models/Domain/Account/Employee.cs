using IdentityProvider.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace IdentityProvider.Models.Domain.Account
{
    /// <inheritdoc />
    [Table("Employee", Schema = "Organization")]
    public class Employee : DomainEntity<int>, IActive
    {
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

        #endregion IsActive


        public ICollection<EmployeeBelongsToOrgUnitLink> OrganizationalUnits { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Employee()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
            OrganizationalUnits = new HashSet<EmployeeBelongsToOrgUnitLink>();
        }

        public List<EmployeeBelongsToOrgUnitLink> FetchAssociatedOrganisationalUnits()
        {
            List<EmployeeBelongsToOrgUnitLink> organisationalUnits;

            try
            {
                organisationalUnits = OrganizationalUnits.Where(i => i.Active && !i.IsDeleted && i.EmployeeId.Equals(Id)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);

                return null;
            }

            return organisationalUnits;
        }
    }
}
