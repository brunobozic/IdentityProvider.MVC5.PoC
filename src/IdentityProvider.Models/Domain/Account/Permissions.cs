using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    /// <summary>
    /// This is a **link** table between the "ApplicationResource" table and the "Operation" table
    /// </summary>
    [Table("Permissions" , Schema = "Resource")]
    public class Permission : DomainEntity<int>, IActive
    {
        public Permission()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive

        public virtual ApplicationResource ApplicationResource { get; set; }
        public virtual Operation Operation { get; set; }

        public int ApplicationResourceId { get; set; }
        public int OperationId { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation
    }
}
