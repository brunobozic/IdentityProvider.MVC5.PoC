using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    /// <summary>
    ///     This is a **link** table between the "ApplicationResource" table and the "Operation" table
    /// </summary>
    [Table("Permissions", Schema = "Resource")]
    public class Permission : DomainEntity<int>, IActive
    {
        public Permission()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [DisplayName("Name")]
        [MaxLength(50, ErrorMessage = "The name of the Operation must be between 2 and 50 characters")]
        [MinLength(2)]
        public string Name { get; set; }

        [DisplayName("Description")]
        [MaxLength(260, ErrorMessage = "The description of the Operation must be between 2 and 260 characters")]
        [MinLength(2)]
        public string Description { get; set; }

        public virtual ApplicationResource ApplicationResource { get; set; }
        public virtual Operation Operation { get; set; }

        public int ApplicationResourceId { get; set; }
        public int OperationId { get; set; }

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        #region IsActive

        [DisplayName("Record is active")] public bool Active { get; set; }

        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }

        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive
    }
}