using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("EmployeeBelongsToOrgUnitLink" , Schema = "Organization")]
    public class EmployeeBelongsToOrgUnitLink : DomainEntity<int>, IActive
    {
        public EmployeeBelongsToOrgUnitLink()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual OrganizationalUnit OrganizationalUnit { get; set; }
        public virtual Employee Employee { get; set; }

        public int OrganizationalUnitId { get; set; }
        public int EmployeeId { get; set; }

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

    }
}
