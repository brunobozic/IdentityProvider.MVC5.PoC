using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityProvider.Infrastructure.Domain;

namespace IdentityProvider.Models.Domain.Account
{
    [Table("RoleResourceJoin" , Schema = "Account")]
   public class RoleResourceJoin : DomainEntity<int>, IActive
    {
        public RoleResourceJoin()
        {
            Active = true;
            ActiveFrom = DateTime.UtcNow;
        }

        public virtual ApplicationRole Role { get; set; }
        public virtual Resource Resource { get; set; }

        public string RoleId { get; set; }
        public int ResourceId { get; set; }

        public override IEnumerable<ValidationResult> Validate( ValidationContext validationContext )
        {
            throw new NotImplementedException();
        }

        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
    }
}
