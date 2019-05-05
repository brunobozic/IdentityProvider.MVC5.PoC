using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{

    [Table("PermissionGroup" , Schema = "Application")]
    public class PermissionGroup : DomainEntity<int>, IActive
    {
        public PermissionGroup()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(260 , ErrorMessage = "The description of the resource permission group unit must be between 5 and 260 characters"), MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(260 , ErrorMessage = "The description of the resource permission group must be between 5 and 260 characters"), MinLength(5)]
        public string Description { get; set; }

        #region IsActive

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive

        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation


    }
}
