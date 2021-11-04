using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("PermissionGroup", Schema = "Resource")]
    public class PermissionGroup : DomainEntity<int>, IActive
    {
        public PermissionGroup()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(260,
            ErrorMessage =
                "The description of the resource permission group unit must be between 5 and 260 characters")]
        [MinLength(5)]
        public string Name { get; set; }


        [MaxLength(260,
            ErrorMessage = "The description of the resource permission group must be between 5 and 260 characters")]
        [MinLength(5)]
        public string Description { get; set; }

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
    }
}