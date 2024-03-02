﻿using Module.CrossCutting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityProvider.Repository.EFCore.Domain.Account
{
    [Table("UserProfile", Schema = "Account")]
    public class UserProfile : DomainEntity<Guid>, IActive
    {
        public UserProfile()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public byte[] ProfilePicture { get; set; }
        public virtual ApplicationUser User { get; set; }

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
        public Guid UserId { get; set; }

        #endregion IsActive
    }
}