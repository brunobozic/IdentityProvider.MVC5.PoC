
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("PermissionGroupOwnsPermissionLink" , Schema = "Application")]
    public class PermissionGroupOwnsPermissionLink : DomainEntity<int>, IActive
    {
        public PermissionGroupOwnsPermissionLink()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual Permission Permission { get; set; }
        public virtual PermissionGroup PermissionGroup { get; set; }

        public int ResourcePermissionId { get; set; }
        public int PermissionGroupId { get; set; }

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
