using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    /// <inheritdoc />
    [Table("Employee" , Schema = "Account")]
    public class Employee : DomainEntity<int>, IActive
    {
        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public ICollection<EmployeeOrgUnitJoin> OrganisationalUnits { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Employee()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
            OrganisationalUnits = new HashSet<EmployeeOrgUnitJoin>();
        }

        public List<EmployeeOrgUnitJoin> FetchAssociatedOrganisationalUnits()
        {
            List<EmployeeOrgUnitJoin> organisationalUnits;

            try
            {
                organisationalUnits = OrganisationalUnits.Where(i => i.Active && !i.IsDeleted && i.EmployeeId.Equals(Id)).ToList();
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
