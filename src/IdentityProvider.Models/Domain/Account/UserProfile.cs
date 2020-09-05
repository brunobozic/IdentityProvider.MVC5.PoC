using IdentityProvider.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("UserProfile", Schema = "Account")]
    public class UserProfile : DomainEntity<int>, IActive
    {
        #region IValidatable Entity contract implementation

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion IValidatable Entity contract implementation

        public byte[] ProfilePicture { get; set; }
        public virtual ApplicationUser User { get; set; }

        #region IsActive

        public bool Active { get; set; }
        [DisplayName("Record is active from (date)")]
        public DateTime? ActiveFrom { get; set; }
        [DisplayName("Record is active to (date)")]
        public DateTime? ActiveTo { get; set; }

        #endregion IsActive

        public UserProfile()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }
    }
}
